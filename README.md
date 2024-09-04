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
### 1. Clone the repository
First, clone the repository to your local machine using the following command:

```bash
git clone https://github.com/minhtruong0208/Thesis.git
cd your-repo
