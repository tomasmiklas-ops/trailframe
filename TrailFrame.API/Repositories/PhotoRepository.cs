using Microsoft.EntityFrameworkCore;
using TrailFrame.API.Data;
using TrailFrame.API.Models;

namespace TrailFrame.API.Repositories;

public class PhotoRepository : IPhotoRepository
{
    private readonly AppDbContext _db;
    public PhotoRepository(AppDbContext db) => _db = db;

    public async Task<Photo?> GetByIdAsync(int id) =>
        await _db.Photos.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Photo>> GetByHikeAsync(int hikeId) =>
        await _db.Photos.Where(p => p.HikeId == hikeId).ToListAsync();

    public async Task<Photo> CreateAsync(Photo photo)
    {
        _db.Photos.Add(photo);
        await _db.SaveChangesAsync();
        return photo;
    }

    public async Task DeleteAsync(int id)
    {
        var photo = await _db.Photos.FindAsync(id);
        if (photo != null) { _db.Photos.Remove(photo); await _db.SaveChangesAsync(); }
    }
}
