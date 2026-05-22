using Microsoft.EntityFrameworkCore;
using TrailFrame.API.Data;
using TrailFrame.API.Models;

namespace TrailFrame.API.Repositories;

public class HikeRepository : IHikeRepository
{
    private readonly AppDbContext _db;
    public HikeRepository(AppDbContext db) => _db = db;

    public async Task<Hike?> GetByIdAsync(int id) =>
        await _db.Hikes.FirstOrDefaultAsync(h => h.Id == id);

    public async Task<IEnumerable<Hike>> GetAllByUserAsync(int userId) =>
        await _db.Hikes.Where(h => h.UserId == userId).OrderByDescending(h => h.Date).ToListAsync();

    // JOIN 1: Hikes + Photos
    public async Task<IEnumerable<object>> GetHikesWithPhotosAsync(int userId) =>
        await _db.Hikes
            .Where(h => h.UserId == userId)
            .Select(h => new
            {
                h.Id, h.Name, h.Area, h.DistanceKm, h.ElevationM, h.Date,
                PhotoCount = _db.Photos.Count(p => p.HikeId == h.Id),
                Photos = _db.Photos.Where(p => p.HikeId == h.Id)
                    .Select(p => new { p.Id, p.Lat, p.Lng, p.FileName }).ToList()
            })
            .ToListAsync<object>();

    // JOIN 2: Hikes + Users
    public async Task<IEnumerable<object>> GetHikesWithUserAsync() =>
        await _db.Hikes
            .Include(h => h.User)
            .Select(h => new
            {
                h.Id, h.Name, h.Area, h.DistanceKm, h.Date, h.Difficulty,
                User = new { h.User!.Id, h.User.Username, h.User.Email }
            })
            .ToListAsync<object>();

    // JOIN 3: Hike + User + Photos
    public async Task<object?> GetHikeDetailAsync(int hikeId) =>
        await _db.Hikes
            .Include(h => h.User)
            .Where(h => h.Id == hikeId)
            .Select(h => new
            {
                h.Id, h.Name, h.Area, h.DistanceKm, h.ElevationM, h.DurationMinutes, h.Date, h.Difficulty,
                User = new { h.User!.Username, h.User.Email },
                Photos = _db.Photos.Where(p => p.HikeId == h.Id)
                    .Select(p => new { p.Id, p.Lat, p.Lng, p.FileName, p.CreatedAt }).ToList()
            })
            .FirstOrDefaultAsync();

    public async Task<Hike> CreateAsync(Hike hike)
    {
        _db.Hikes.Add(hike);
        await _db.SaveChangesAsync();
        return hike;
    }

    public async Task<Hike> UpdateAsync(Hike hike)
    {
        _db.Hikes.Update(hike);
        await _db.SaveChangesAsync();
        return hike;
    }

    public async Task DeleteAsync(int id)
    {
        var hike = await _db.Hikes.FindAsync(id);
        if (hike != null) { _db.Hikes.Remove(hike); await _db.SaveChangesAsync(); }
    }
}
