import subprocess
from pathlib import Path
import cv2
from app.database import get_students_collection, get_unknown_collection

def convert_video_to_mp4(input_path, output_path):
    ffmpeg_cmd = [
        "ffmpeg",
        "-i", input_path,
        "-c:v", "libx264",
        "-preset", "slower",
        "-crf", "22",
        "-vf", "scale=trunc(iw/2)*2:trunc(ih/2)*2",
        "-c:a", "copy",
        output_path
    ]
    subprocess.run(ffmpeg_cmd, check=True)

def get_videos_dir():
    videos_dir = Path.joinpath(Path.cwd(), "AttendanceSystemAPI/videos")
    videos_dir.mkdir(parents=True, exist_ok=True)
    return videos_dir

def get_video_duration(video_entry_id, videos_collection):
    video_entry = videos_collection.find_one({'_id': video_entry_id})
    video_path = video_entry['input_video_path']
    cap = cv2.VideoCapture(video_path)
    fps = cap.get(cv2.CAP_PROP_FPS)
    total_frames = cap.get(cv2.CAP_PROP_FRAME_COUNT)
    duration = total_frames / fps
    cap.release()
    return duration

def get_attendance_status(percentage):
    if percentage > 80:
        return "present"
    elif 50 <= percentage <= 80:
        return "warning"
    else:
        return "absent"

def update_students_collection(results, current_date):
    students_collection = get_students_collection()
    all_students = list(students_collection.find({}))
    recognized_students = {result['name']: result for result in results}
    
    for student in all_students:
        student_name = student["name"]
        attendance_records = student.get("attendance_records", [])
        
        if student_name in recognized_students:
            attendance_record = {
                "date": current_date,
                "status": recognized_students[student_name]["attendance_status"]
            }
        else:
            attendance_record = {
                "date": current_date,
                "status": "absent"
            }
        
        attendance_records.append(attendance_record)
        students_collection.update_one(
            {"name": student_name},
            {"$set": {"attendance_records": attendance_records}}
        )

    unknown_collection = get_unknown_collection()
    unknown_persons = [result for result in results if result['name'].startswith('Unknown_')]
    
    for person in unknown_persons:
        unknown_collection.insert_one({
            "date": current_date,
            "appearance_time": person["total_appearance_time"],
            "percentage": person["percentage"]
        })

def post_process_unknown_faces(unknown_faces, fps, time_threshold=1.0):
    sorted_unknowns = sorted(unknown_faces, key=lambda x: x['start_frame'])
    merged_unknowns = []
    current_unknown = None

    for unknown in sorted_unknowns:
        if not current_unknown:
            current_unknown = unknown
        elif unknown['start_frame'] - current_unknown['end_frame'] <= time_threshold * fps:
            current_unknown['end_frame'] = max(current_unknown['end_frame'], unknown['end_frame'])
        else:
            merged_unknowns.append(current_unknown)
            current_unknown = unknown

    if current_unknown:
        merged_unknowns.append(current_unknown)

    return merged_unknowns