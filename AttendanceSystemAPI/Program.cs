using AttendanceSystemAPI.Models;
using AttendanceSystemAPI.Services;
using Microsoft.Extensions.FileProviders;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ và cấu hình middleware
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .WithOrigins("http://localhost:8080")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Thiết lập kết nối MongoDB
builder.Services.AddSingleton<IMongoClient>(s =>
{
    var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDBSettings>();
    return new MongoClient(settings.ConnectionString);
});

// Đăng ký các service
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddHttpClient<IVideoService, VideoService>()
    .ConfigureHttpClient(client => 
    {
        client.Timeout = TimeSpan.FromHours(2);
    });
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IClassService, ClassService>();

var app = builder.Build();

// Cấu hình HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "videos")),
    RequestPath = "/videos"
});

var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "Data", "UnknownFaces");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(path),
    RequestPath = "/unknown-faces"
});


var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Data");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(dataPath),
    RequestPath = "/Data"
});

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.MapControllers();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
