import os
from datetime import datetime
from collections import defaultdict
from itertools import groupby
import cv2
from deepface import DeepFace
from app.utils import get_video_duration, get_attendance_status, update_students_collection, post_process_unknown_faces
from app.config import CONFIG
from app.database import get_unknown_collection

def process_cropped_faces_helper(cropped_faces_dir, database_dir, videos_collection, video_entry_id):
    threshold = 0.30
    fps = 30
    person_info = defaultdict(lambda: {"ids": defaultdict(lambda: {"start_frame": float('inf'), "end_frame": 0, "count": 0})})
    all_unknown_faces = []
    
    cropped_faces_paths = [os.path.join(cropped_faces_dir, face_file) for face_file in os.listdir(cropped_faces_dir)]
    batch_size = 8
    batches = [cropped_faces_paths[i:i+batch_size] for i in range(0, len(cropped_faces_paths), batch_size)]
    
    for batch in batches:
        all_unknown_faces.extend(process_batch(batch, database_dir, person_info, threshold))
    
    all_unknown_faces.sort(key=lambda x: x["obj_id"])
    merged_unknown_faces = []
    for obj_id, group in groupby(all_unknown_faces, key=lambda x: x["obj_id"]):
        group = list(group)
        merged_unknown_faces.append({
            "obj_id": obj_id,
            "start_frame": min(face["start_frame"] for face in group),
            "end_frame": max(face["end_frame"] for face in group)
        })

    merged_unknown_faces = post_process_unknown_faces(merged_unknown_faces, fps)

    unknown_count = len(merged_unknown_faces)
    
    current_date = datetime.now().date().isoformat()
    video_duration = get_video_duration(video_entry_id, videos_collection)
    results = []
    for person, info in person_info.items():
        total_appearance_time = sum((data["end_frame"] - data["start_frame"] + 1) / fps for data in info["ids"].values())
        percentage = min((total_appearance_time / video_duration) * 100, 100)
        attendance_status = get_attendance_status(percentage)
        
        result = {
            "name": person,
            "total_appearance_time": total_appearance_time,
            "percentage": percentage,
            "date": current_date,
            "attendance_status": attendance_status
        }
        results.append(result)

    unknown_persons = []
    for idx, unknown in enumerate(merged_unknown_faces):
        appearance_time = (unknown["end_frame"] - unknown["start_frame"] + 1) / fps
        percentage = min((appearance_time / video_duration) * 100, 100)
        unknown_person = {
            "name": f"Unknown_{idx+1}",
            "total_appearance_time": appearance_time,
            "percentage": percentage,
            "date": current_date,
            "attendance_status": "unknown",
            "image_path": f"/unknown-faces/Unknown_{unknown['obj_id']}.jpg"
        }
        results.append(unknown_person)
        unknown_persons.append(unknown_person)

    videos_collection.update_one(
        {'_id': video_entry_id}, 
        {
            '$set': {
                'match_results': results, 
                'processed': True,
                'unknown_count': unknown_count
            }
        }
    )
    update_students_collection(results, current_date)

    unknown_collection = get_unknown_collection()
    for person in unknown_persons:
        unknown_collection.insert_one({
            "date": current_date,
            "appearance_time": person["total_appearance_time"],
            "percentage": person["percentage"],
            "image_path": person["image_path"]
        })

def process_batch(batch, database_dir, person_info, threshold):
    potential_unknowns = defaultdict(list)
    for face_path in batch:
        face_file = os.path.basename(face_path)
        frame_number = int(face_file.split("_")[-1].split(".")[0])
        obj_id = int(face_file.split("_")[1])

        matches = DeepFace.find(img_path=face_path, db_path=database_dir, model_name='DeepID', enforce_detection=False)

        if matches and len(matches) > 0:
            min_distance = float('inf')
            best_match = None
            for data_frame in matches:
                for _, row in data_frame.iterrows():
                    cosine_distance = row['distance']
                    if cosine_distance < min_distance:
                        min_distance = cosine_distance
                        best_match = os.path.basename(os.path.dirname(row['identity']))

            if min_distance <= threshold:
                if best_match not in person_info:
                    person_info[best_match] = {"ids": {}}
                if obj_id not in person_info[best_match]["ids"]:
                    person_info[best_match]["ids"][obj_id] = {"start_frame": frame_number, "end_frame": frame_number, "count": 0}
                person_info[best_match]["ids"][obj_id]["count"] += 1
                person_info[best_match]["ids"][obj_id]["start_frame"] = min(person_info[best_match]["ids"][obj_id]["start_frame"], frame_number)
                person_info[best_match]["ids"][obj_id]["end_frame"] = max(person_info[best_match]["ids"][obj_id]["end_frame"], frame_number)
            else:
                potential_unknowns[obj_id].append((frame_number, face_path))
        else:
            potential_unknowns[obj_id].append((frame_number, face_path))

    saved_unknown_faces = set()
    unknown_faces = []
    for obj_id, frames in potential_unknowns.items():
        if obj_id not in person_info:
            first_frame_path = frames[0][1]
            unknown_face_filename = f"Unknown_{obj_id}.jpg"
            unknown_face_path = os.path.join(CONFIG.UNKNOWN_FACES_DIR, unknown_face_filename)

            if obj_id not in saved_unknown_faces:
                original_image = cv2.imread(first_frame_path)
                cv2.imwrite(unknown_face_path, original_image)
                saved_unknown_faces.add(obj_id)

            unknown_faces.append({
                "obj_id": obj_id,
                "start_frame": min(frames)[0], 
                "end_frame": max(frames)[0],   
                "face_path": f"/unknown-faces/Unknown_{obj_id}.jpg"
            })

    return unknown_faces