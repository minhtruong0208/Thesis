from pymongo import MongoClient
from app.config import CONFIG

client = None
db = None

def init_db(app):
    global client, db
    client = MongoClient(CONFIG.MONGO_URI)
    db = client[CONFIG.DATABASE_NAME]

def get_videos_collection():
    return db['videos']

def get_students_collection():
    return db['students']

def get_unknown_collection():
    return db['unknown_persons']