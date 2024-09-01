using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace AttendanceSystemAPI.Models
{
    public class Student
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("name")]
        public string Name { get; set; } = null!;
        [BsonElement("face_path")]
        public string FacePath { get; set; } = null!;
        [BsonElement("class_id")]
        public string ClassId { get; set; } = null!;
        [BsonElement("email")]
        public string? Email { get; set; }
        [BsonElement("attendance_records")]
        public List<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    }

    public class AttendanceRecord
    {
        [BsonElement("date")]
        public string Date { get; set; }
        [BsonElement("status")]
        public string Status { get; set; }
    }
}