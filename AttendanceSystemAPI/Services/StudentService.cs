using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AttendanceSystemAPI.Models;
using MongoDB.Driver;

namespace AttendanceSystemAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IMongoCollection<Student> _studentsCollection;

        public StudentService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("AttendanceSystem");
            _studentsCollection = database.GetCollection<Student>("students");
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _studentsCollection.Find(FilterDefinition<Student>.Empty).ToListAsync();
        }

        public async Task UpdateStudentAsync(string id, Student updatedStudent)
        {
            var filter = Builders<Student>.Filter.Eq(s => s.Id, id);

            var updateDefinitions = new List<UpdateDefinition<Student>>
            {
                Builders<Student>.Update.Set(s => s.Name, updatedStudent.Name),
                Builders<Student>.Update.Set(s => s.ClassId, updatedStudent.ClassId)
            };

            if (updatedStudent.AttendanceRecords != null)
            {
                foreach (var record in updatedStudent.AttendanceRecords)
                {
                    var updateDefinition = Builders<Student>.Update
                        .Set("AttendanceRecords.$.Status", record.Status)
                        .Set("AttendanceRecords.$.Date", record.Date);

                    var arrayFilter = Builders<Student>.Filter
                        .ElemMatch(s => s.AttendanceRecords, r => r.Date == record.Date);

                    await _studentsCollection.UpdateOneAsync(
                        Builders<Student>.Filter.And(
                            filter,
                            arrayFilter
                        ),
                        updateDefinition
                    );
                }
            }

            var combinedUpdate = Builders<Student>.Update.Combine(updateDefinitions);

            await _studentsCollection.UpdateOneAsync(filter, combinedUpdate);
        }

        public async Task<string> GetStudentImageAsync(string studentName)
        {
            var student = await _studentsCollection.Find(s => s.Name == studentName).FirstOrDefaultAsync();

            if (student == null || string.IsNullOrEmpty(student.FacePath))
            {
                return null;
            }

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "..", student.FacePath);
            if (!Directory.Exists(fullPath))
            {
                return null;
            }

            var imageFiles = Directory.GetFiles(fullPath, "*.jpg");
            if (imageFiles.Length == 0)
            {
                return null;
            }

            return imageFiles[0];
        }
    }
}