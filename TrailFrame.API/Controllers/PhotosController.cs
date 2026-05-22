using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using TrailFrame.API.Data;
using TrailFrame.API.Models;

namespace TrailFrame.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PhotosController : ControllerBase
{
    private readonly AppDbContext _db;
    public PhotosController(AppDbContext db) => _db = db;
    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost("upload")]
    [RequestSizeLimit(50_000_000)]
    public async Task<IActionResult> Upload()
    {
        var form = await Request.ReadFormAsync();
        
        var file = form.Files.GetFile("file");
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Žiadny súbor." });

        if (!double.TryParse(form["lat"], NumberStyles.Float, CultureInfo.InvariantCulture, out double lat) ||
            !double.TryParse(form["lng"], NumberStyles.Float, CultureInfo.InvariantCulture, out double lng))
            return BadRequest(new { message = "Neplatné GPS." });

        int.TryParse(form["hikeId"], out int hikeId);

        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
            await file.CopyToAsync(stream);

        var photo = new Photo
        {
            FileName = fileName,
            Lat = lat,
            Lng = lng,
            HikeId = hikeId,
            UserId = GetUserId()
        };

        _db.Photos.Add(photo);
        await _db.SaveChangesAsync();

        return Ok(new { photo.Id, photo.Lat, photo.Lng, url = $"/uploads/{fileName}" });
    }

    [HttpGet("hike/{hikeId}")]
    public IActionResult GetForHike(int hikeId)
    {
        var photos = _db.Photos
            .Where(p => p.HikeId == hikeId)
            .Select(p => new { p.Id, p.Lat, p.Lng, url = $"/uploads/{p.FileName}", p.CreatedAt })
            .ToList();
        return Ok(photos);
    }
}