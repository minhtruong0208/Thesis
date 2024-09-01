from flask import Flask
from app.config import CONFIG
from app.database import init_db
from app.video_processor import video_bp

app = Flask(__name__)
app.config.from_object(CONFIG)

init_db(app)
app.register_blueprint(video_bp)

if __name__ == '__main__':
    app.run(debug=True)