import os

class Config:
    MONGO_URI = 'mongodb://localhost:27017/'
    DATABASE_NAME = 'AttendanceSystem'
    VIDEO_TEMP_DIR = 'Data/Temp'
    OUTPUT_DIR = 'Data/Output'
    MODEL_PATH = 'Model/yolov8n-face.pt'
    DATABASE_DIR = 'Data/Database'
    UNKNOWN_FACES_DIR = 'Data/UnknownFaces'

CONFIG = Config()