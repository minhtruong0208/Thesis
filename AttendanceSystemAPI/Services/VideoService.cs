using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AttendanceSystemAPI.Services
{
    public class VideoService : IVideoService
    {
        private readonly IMongoCollection<BsonDocument> _videosCollection;
        private readonly HttpClient _httpClient;

        public VideoService(IMongoClient mongoClient, HttpClient httpClient)
        {
            var database = mongoClient.GetDatabase("AttendanceSystem");
            _videosCollection = database.GetCollection<BsonDocument>("videos");
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromHours(2);
        }

        public async Task<string> UploadVideoAsync(IFormFile videoFile)
        {
            var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "videos", videoFile.FileName);
            using (var stream = new FileStream(videoPath, FileMode.Create))
            {
                await videoFile.CopyToAsync(stream);
            }

            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new StreamContent(File.OpenRead(videoPath));
                content.Add(fileContent, "video", videoFile.FileName);

                var response = await _httpClient.PostAsync("http://localhost:5000/process_video", content);
                response.EnsureSuccessStatusCode();
            }

            var videoEntry = new BsonDocument
            {
                { "filename", videoFile.FileName },
                { "input_video_path", videoPath },
                { "processed", false }
            };

            await _videosCollection.InsertOneAsync(videoEntry);

            return videoFile.FileName;
        }

        public async Task<string> GetVideoResultAsync(string filename)
        {
            var outputVideoPath = Path.Combine("videos", $"processed_{filename}");
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), outputVideoPath);
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Processed video not found: {outputVideoPath}");
            }
            return outputVideoPath; 
        }

        public async Task ProcessCroppedFacesAsync(string filename)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("filename", filename);
            var video = await _videosCollection.Find(filter).FirstOrDefaultAsync();

            if (video == null)
            {
                throw new Exception("Video not found");
            }

            using (var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("video_filename", filename)
            }))
            {
                var response = await _httpClient.PostAsync("http://localhost:5000/process_cropped_faces", content);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}