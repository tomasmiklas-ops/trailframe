using Microsoft.EntityFrameworkCore;
using TrailFrame.API.Data;
using TrailFrame.API.Models;

namespace TrailFrame.API;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Users.AnyAsync()) return; // už sú dáta

        var users = new List<User>
        {
            new User { Username = "janko_hiker", Email = "janko@trailframe.sk", PasswordHash = BCrypt.Net.BCrypt.HashPassword("heslo123"), CreatedAt = DateTime.UtcNow.AddMonths(-6) },
            new User { Username = "maria_peaks", Email = "maria@trailframe.sk", PasswordHash = BCrypt.Net.BCrypt.HashPassword("heslo123"), CreatedAt = DateTime.UtcNow.AddMonths(-3) },
            new User { Username = "peter_trails", Email = "peter@trailframe.sk", PasswordHash = BCrypt.Net.BCrypt.HashPassword("heslo123"), CreatedAt = DateTime.UtcNow.AddMonths(-1) },
        };
        db.Users.AddRange(users);
        await db.SaveChangesAsync();

        var hikes = new List<Hike>
        {
            new Hike { Name = "Veľký Kriváň", Area = "Malá Fatra", DistanceKm = 14.2, ElevationM = 980, DurationMinutes = 262, Difficulty = "Ťažká", Date = DateTime.UtcNow.AddDays(-30), UserId = users[0].Id, TrackJson = "[[49.155,18.920],[49.2023,18.9741]]", PhotosJson = "[]" },
            new Hike { Name = "Rysy", Area = "Vysoké Tatry", DistanceKm = 11.5, ElevationM = 1180, DurationMinutes = 348, Difficulty = "Ťažká", Date = DateTime.UtcNow.AddDays(-15), UserId = users[0].Id, TrackJson = "[[49.175,20.045],[49.2075,20.0961]]", PhotosJson = "[]" },
            new Hike { Name = "Chopok", Area = "Nízke Tatry", DistanceKm = 9.8, ElevationM = 650, DurationMinutes = 185, Difficulty = "Stredná", Date = DateTime.UtcNow.AddDays(-45), UserId = users[1].Id, TrackJson = "[[48.920,19.585],[48.965,19.640]]", PhotosJson = "[]" },
            new Hike { Name = "Kriváň", Area = "Vysoké Tatry", DistanceKm = 12.4, ElevationM = 1050, DurationMinutes = 300, Difficulty = "Ťažká", Date = DateTime.UtcNow.AddDays(-7), UserId = users[1].Id, TrackJson = "[[49.160,19.985],[49.195,20.020]]", PhotosJson = "[]" },
            new Hike { Name = "Poludňový grúň", Area = "Malá Fatra", DistanceKm = 6.2, ElevationM = 420, DurationMinutes = 140, Difficulty = "Ľahká", Date = DateTime.UtcNow.AddDays(-3), UserId = users[2].Id, TrackJson = "[[49.180,18.850],[49.200,18.880]]", PhotosJson = "[]" },
        };
        db.Hikes.AddRange(hikes);
        await db.SaveChangesAsync();

        var photos = new List<Photo>
        {
            new Photo { FileName = "sample1.jpg", Lat = 49.162, Lng = 18.930, HikeId = hikes[0].Id, UserId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-30) },
            new Photo { FileName = "sample2.jpg", Lat = 49.188, Lng = 18.965, HikeId = hikes[0].Id, UserId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-30) },
            new Photo { FileName = "sample3.jpg", Lat = 49.2023, Lng = 18.9741, HikeId = hikes[0].Id, UserId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-30) },
            new Photo { FileName = "sample4.jpg", Lat = 49.178, Lng = 20.050, HikeId = hikes[1].Id, UserId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-15) },
            new Photo { FileName = "sample5.jpg", Lat = 49.2075, Lng = 20.0961, HikeId = hikes[1].Id, UserId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-15) },
            new Photo { FileName = "sample6.jpg", Lat = 48.965, Lng = 19.640, HikeId = hikes[2].Id, UserId = users[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-45) },
        };
        db.Photos.AddRange(photos);
        await db.SaveChangesAsync();
    }
}
