using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceSystemAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AttendanceSystemAPI.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IMongoCollection<BsonDocument> _videosCollection;
        private readonly IMongoCollection<Student> _studentsCollection;
        private readonly IEmailService _emailService;

        public AttendanceService(IMongoClient mongoClient, IEmailService emailService)
        {
            var database = mongoClient.GetDatabase("AttendanceSystem");
            _videosCollection = database.GetCollection<BsonDocument>("videos");
            _studentsCollection = database.GetCollection<Student>("students");
            _emailService = emailService;
        }

        public async Task<(List<Person> persons, bool emailsSent, int emailsSentCount, int unknownCount)> GetAttendanceResultAsync(string filename)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("filename", filename);
            var video = await _videosCollection.Find(filter).FirstOrDefaultAsync();

            if (video == null)
            {
                throw new Exception("Video not found");
            }

            var matchResults = video["match_results"].AsBsonArray;
            var unknownCount = video["unknown_count"].AsInt32;

            var students = await _studentsCollection.Find(FilterDefinition<Student>.Empty).ToListAsync();

            var persons = new List<Person>();

            foreach (var student in students)
            {
                var attendanceRecord = matchResults.FirstOrDefault(r => r["name"].AsString == student.Name);
                if (attendanceRecord != null)
                {
                    persons.Add(new Person
                    {
                        Name = student.Name,
                        AttendanceStatus = attendanceRecord["attendance_status"].AsString,
                        AttendanceDate = attendanceRecord["date"].AsString,
                        AppearanceTime = ConvertToDouble(attendanceRecord["total_appearance_time"]),
                        AppearancePercentage = ConvertToDouble(attendanceRecord["percentage"]),
                        ImagePath = $"/api/home/student-image/{student.Name}"
                    });
                }
                else
                {
                    persons.Add(new Person
                    {
                        Name = student.Name,
                        AttendanceStatus = "absent",
                        AttendanceDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        AppearanceTime = 0,
                        AppearancePercentage = 0,
                        ImagePath = $"/api/home/student-image/{student.Name}"
                    });
                }
            }

            var unknownPersons = matchResults.Where(r => r["name"].AsString.StartsWith("Unknown_"))
                .Select(r => new Person
                {
                    Name = r["name"].AsString,
                    AttendanceStatus = "unknown",
                    AttendanceDate = r["date"].AsString,
                    AppearanceTime = ConvertToDouble(r["total_appearance_time"]),
                    AppearancePercentage = ConvertToDouble(r["percentage"]),
                    ImagePath = r["image_path"].AsString
                }).ToList();

            persons.AddRange(unknownPersons);

            var emailsSent = false;
            var emailsSentCount = 0;

            foreach (var person in persons.Where(p => p.AttendanceStatus == "absent"))
            {
                var student = students.FirstOrDefault(s => s.Name == person.Name);
                if (student != null && !string.IsNullOrEmpty(student.Email))
                {
                    try
                    {
                        await SendAbsentNotificationEmail(student.Email, student.Name, person.AttendanceDate);
                        emailsSent = true;
                        emailsSentCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email to {student.Email}. Error: {ex.Message}");
                    }
                }
            }

            var update = Builders<BsonDocument>.Update
                .Set("last_email_sent", DateTime.UtcNow)
                .Set("emails_sent_count", emailsSentCount);
            await _videosCollection.UpdateOneAsync(filter, update);

            return (persons, emailsSent, emailsSentCount, unknownCount);
        }

        private double ConvertToDouble(BsonValue value)
        {
            if (value.IsDouble)
                return value.AsDouble;
            else if (value.IsInt32)
                return (double)value.AsInt32;
            else if (value.IsInt64)
                return (double)value.AsInt64;
            else
                return 0;
        }

        private async Task SendAbsentNotificationEmail(string email, string name, string date)
        {
            string subject = "Thông báo vắng mặt";
            string body = $"Xin chào {name},\n\nBạn đã được đánh dấu vắng mặt trong buổi học ngày {date}. Vui lòng liên hệ với giáo viên để biết thêm chi tiết.\n\nTrân trọng,\nHệ thống điểm danh tự động";

            await _emailService.SendEmailAsync(email, subject, body);
        }
    }
}