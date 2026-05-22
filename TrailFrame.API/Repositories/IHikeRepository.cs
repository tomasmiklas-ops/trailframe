using TrailFrame.API.Models;

namespace TrailFrame.API.Repositories;

public interface IHikeRepository
{
    Task<Hike?> GetByIdAsync(int id);
    Task<IEnumerable<Hike>> GetAllByUserAsync(int userId);
    Task<IEnumerable<object>> GetHikesWithPhotosAsync(int userId);
    Task<IEnumerable<object>> GetHikesWithUserAsync();
    Task<object?> GetHikeDetailAsync(int hikeId);
    Task<Hike> CreateAsync(Hike hike);
    Task<Hike> UpdateAsync(Hike hike);
    Task DeleteAsync(int id);
}
