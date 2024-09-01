namespace AttendanceSystemAPI.Models
{
    public class Person
    {
        public string Name { get; set; } = null!;
        public double AppearanceTime { get; set; }
        public string AttendanceStatus { get; set; } = null!;
        public string AttendanceDate { get; set; } = null!;
        public double AppearancePercentage { get; set; }
        public string? ImagePath { get; set; }
    }
}
