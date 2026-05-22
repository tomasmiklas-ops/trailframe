using TrailFrame.API.Models;

namespace TrailFrame.API.Repositories;

public interface IPhotoRepository
{
    Task<Photo?> GetByIdAsync(int id);
    Task<IEnumerable<Photo>> GetByHikeAsync(int hikeId);
    Task<Photo> CreateAsync(Photo photo);
    Task DeleteAsync(int id);
}
