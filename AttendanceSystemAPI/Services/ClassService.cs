using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AttendanceSystemAPI.Services
{
    public class ClassService : IClassService
    {
        private readonly IMongoCollection<BsonDocument> _classesCollection;

        public ClassService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("AttendanceSystem");
            _classesCollection = database.GetCollection<BsonDocument>("classes");
        }

        public async Task<string> GetClassNameAsync(string classId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", classId);
            var classDocument = await _classesCollection.Find(filter).FirstOrDefaultAsync();

            if (classDocument == null)
            {
                return null;
            }

            return classDocument["class_name"].AsString;
        }
    }
}