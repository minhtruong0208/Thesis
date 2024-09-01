using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttendanceSystemAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = string.Empty;

        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Roles { get; set; } = null!;
    }
}
