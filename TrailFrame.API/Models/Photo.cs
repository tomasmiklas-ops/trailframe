namespace TrailFrame.API.Models;

public class Photo
{
    public int Id { get; set; }
    public string FileName { get; set; } = "";
    public double Lat { get; set; }
    public double Lng { get; set; }
    public int HikeId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
