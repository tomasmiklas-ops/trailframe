namespace TrailFrame.API.Models;

public class Hike
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Area { get; set; } = "";
    public double DistanceKm { get; set; }
    public int ElevationM { get; set; }
    public int DurationMinutes { get; set; }
    public string Difficulty { get; set; } = "Stredná";
    public DateTime Date { get; set; } = DateTime.UtcNow;

    // GPS track stored as JSON string: [[lat,lng],[lat,lng],...]
    public string TrackJson { get; set; } = "[]";

    // Photo pins stored as JSON string: [{lat,lng,name,location}]
    public string PhotosJson { get; set; } = "[]";

    // Foreign key
    public int UserId { get; set; }
    public User? User { get; set; }
}
