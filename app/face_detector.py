import os
import json
import cv2
import numpy as np
import torch
from ultralytics import YOLO
from app.utils import convert_video_to_mp4, get_videos_dir
from app.config import CONFIG

def process_video_file(video_path, video):
    model = YOLO(CONFIG.MODEL_PATH)
    cap = cv2.VideoCapture(video_path)
    width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
    height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
    fps = cap.get(cv2.CAP_PROP_FPS)

    output_dir = CONFIG.OUTPUT_DIR
    os.makedirs(output_dir, exist_ok=True)
    cropped_faces_dir = os.path.join(output_dir, "cropped_faces")
    os.makedirs(cropped_faces_dir, exist_ok=True)

    temp_video_path = os.path.join(output_dir, "temp_annotated_video.avi")
    fourcc = cv2.VideoWriter_fourcc(*'DIVX')
    video_writer = cv2.VideoWriter(temp_video_path, fourcc, fps, (width, height))

    id_info = {}

    while cap.isOpened():
        success, frame = cap.read()
        if success:
            results = model.track(frame, persist=True)
            for box in results[0].boxes:
                process_detected_object(box, frame, video_writer, cropped_faces_dir, cap, id_info)
            annotated_frame = results[0].plot()
            video_writer.write(annotated_frame)
        else:
            break

    cap.release()
    video_writer.release()
    cv2.destroyAllWindows()

    id_info_file = os.path.join(output_dir, "id_info.json")
    with open(id_info_file, "w") as f:
        json.dump(id_info, f, indent=4)

    output_video_path = os.path.join(output_dir, "annotated_video.mp4")
    convert_video_to_mp4(temp_video_path, output_video_path)
    os.remove(temp_video_path)

    videos_dir = get_videos_dir()
    videos_output_path = videos_dir.joinpath(f"processed_{video.filename}")
    os.replace(output_video_path, str(videos_output_path))

    return output_dir, cropped_faces_dir, video_path

def process_detected_object(box, frame, video_writer, cropped_faces_dir, cap, id_info):
    x1, y1, x2, y2 = box.xyxy[0].cpu().numpy().astype(int)
    obj_id = int(box.id.item())
    current_frame = int(cap.get(cv2.CAP_PROP_POS_FRAMES))

    if obj_id not in id_info or current_frame - id_info[obj_id]['last_frame'] >= 30:
        cropped_face = frame[y1:y2, x1:x2]
        cropped_face = cv2.resize(cropped_face, (55, 47))
        face_path = os.path.join(cropped_faces_dir, f"ID_{obj_id}_frame_{current_frame}.jpg")
        cv2.imwrite(face_path, cropped_face)

        if obj_id not in id_info:
            id_info[obj_id] = {
                'start_frame': current_frame,
                'end_frame': current_frame,
                'last_frame': current_frame,
                'person': None
            }
        else:
            id_info[obj_id]['end_frame'] = current_frame
            id_info[obj_id]['last_frame'] = current_frame