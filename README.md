# AI-Based Student Attendance System on videos

This project is an AI-powered student attendance system that uses video footage to recognize students' faces and automates the attendance process. It makes use of a variety of cutting-edge technologies, including Python, C#, JavaScript, and frameworks like ASP.NET Core, Flask, and Vue. This project was build for my graduation thesis at HCMC University of Science, scored 9.8/10 by thesis committee.

## Table of Contents
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)

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
