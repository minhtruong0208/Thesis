import os
from flask import Blueprint, request
from app.face_detector import process_video_file
from app.face_recognizer import process_cropped_faces_helper
from app.database import get_videos_collection
from app.config import CONFIG

video_bp = Blueprint('video', __name__)

@video_bp.route('/process_video', methods=['POST'])
def process_video():
    video = request.files['video']
    video_path = os.path.join(CONFIG.VIDEO_TEMP_DIR, video.filename)
    video.save(video_path)

    output_dir, cropped_faces_dir, input_video_path = process_video_file(video_path, video)

    videos_collection = get_videos_collection()
    video_entry = {
        'filename': video.filename,
        'input_video_path': input_video_path,
        'output_dir': output_dir,
        'cropped_faces_dir': cropped_faces_dir,
        'processed': False
    }
    video_entry_id = videos_collection.insert_one(video_entry).inserted_id
    return 'Video processed successfully'

@video_bp.route('/process_cropped_faces', methods=['POST'])
def process_cropped_faces():
    video_filename = request.form.get('video_filename')
    
    if not video_filename:
        return 'Missing video_filename parameter', 400
    
    videos_collection = get_videos_collection()
    video_entry = videos_collection.find_one({'filename': video_filename, 'processed': False})

    if video_entry:
        cropped_faces_dir = video_entry['cropped_faces_dir']
        process_cropped_faces_helper(cropped_faces_dir, CONFIG.DATABASE_DIR, videos_collection, video_entry['_id'])
        return 'Cropped faces processed successfully'
    else:
        return 'No video found or video already processed', 404