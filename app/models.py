from datetime import datetime

class VideoEntry:
    def __init__(self, filename, input_video_path, output_dir, cropped_faces_dir):
        self.filename = filename
        self.input_video_path = input_video_path
        self.output_dir = output_dir
        self.cropped_faces_dir = cropped_faces_dir
        self.processed = False

class AttendanceRecord:
    def __init__(self, name, total_appearance_time, percentage, date, attendance_status):
        self.name = name
        self.total_appearance_time = total_appearance_time
        self.percentage = percentage
        self.date = date
        self.attendance_status = attendance_status

class UnknownPerson:
    def __init__(self, date, appearance_time, percentage, image_path):
        self.date = date
        self.appearance_time = appearance_time
        self.percentage = percentage
        self.image_path = image_path