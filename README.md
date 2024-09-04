# AI-Based Student Attendance System on videos

This project is an AI-powered student attendance system that uses video footage to recognize students' faces and automates the attendance process. It makes use of a variety of cutting-edge technologies, including Python, C#, JavaScript, and frameworks like ASP.NET Core, Flask, and Vue. This project was build for my graduation thesis at HCMC University of Science, scored 9.8/10 by thesis committee.

## Table of Contents
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)

## Features
- Detecting and tracking faces in video frames
- Identifies students based on pre-registered facial features using DeepFace.
- Using STMP Gmail to send email to absent students.
- Calculates the duration of each student's presence and records attendance.
- User-friendly web interface for interaction and visualization

## Technologies Used
- Backend:
  - Python with Flask
  - C# with ASP.NET Core
- Frontend:
  - JavaScript with Vue.js
- YOLOv8 (for face detection), DeepFace (for face recognition)
- Database: MongoDB

## Installation
### Prerequisites
- Python 3.10.11
- .NET Core 8
- NodeJS 20.12.2

1. Clone the repository:
   ```
   git clone https://github.com/minhtruong0208/ai-powered-attendance-system.git
   ```
2. Set up the Python environment:
   ```
   cd ai-powered-attendance-system/app
   python -m venv venv
   pip install -r requirements.txt
   ```
3. Set up the ASP.NET Core project:
   ```
   cd ai-powered-attendance-system/AttendanceSystemAPI
   dotnet restore
   ```
4. Set up the Vue.js project:
   ```
   cd ai-powered-attendance-system/client-app
   npm install
   ```
## Usage
### Data Organization
The system requires a specific data structure for storing reference images used in face recognition:

1. All reference images are stored in the `Data/Database` directory.
2. Within this directory, create a separate folder for each person.
3. Name each folder with the person's name or unique identifier.
4. Place the reference images for each person in their respective folder.

Example structure:
```
Data/
├── Database/
│   ├── John_Doe/
│   │   ├── image1.jpg
│   │   ├── image2.jpg
│   │   └── image3.jpg
│   ├── Jane_Smith/
│   │   ├── image1.jpg
│   │   └── image2.jpg
│   └── Alice_Johnson/
│       ├── image1.jpg
│       ├── image2.jpg
│       └── image3.jpg
```
Or you can watch in Data folder to known how data is organized.
### MongoDB Database
The structure of collections in MongoDB in this project is shown in collections_schema.json. You can create your database in MongoDB, mine is AttendanceSystem. Make sure the collections is similar to the collections_schema.json file.

### Run
1. Start the Flask server:
   ```
   python app.py
   ```

2. Start the ASP.NET Core server:
   ```
   dotnet run
   ```

3. Start the Vue.js development server:
   ```
   npm run serve
   ```
