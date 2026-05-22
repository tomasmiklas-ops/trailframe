using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrailFrame.API.Data;
using TrailFrame.API.DTOs;
using TrailFrame.API.Models;

namespace TrailFrame.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HikesController : ControllerBase
{
    private readonly AppDbContext _db;

    public HikesController(AppDbContext db) => _db = db;

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET /api/hikes — get all hikes for logged-in user
    [HttpGet]
    public async Task<IActionResult> GetMyHikes()
    {
        var userId = GetUserId();
        var hikes = await _db.Hikes
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.Date)
            .Select(h => new
            {
                h.Id, h.Name, h.Area, h.DistanceKm,
                h.ElevationM, h.DurationMinutes, h.Difficulty,
                h.Date, h.TrackJson, h.PhotosJson
            })
            .ToListAsync();
        return Ok(hikes);
    }

    // GET /api/hikes/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetHike(int id)
    {
        var userId = GetUserId();
        var hike = await _db.Hikes.FirstOrDefaultAsync(h => h.Id == id && h.UserId == userId);
        if (hike == null) return NotFound();
        return Ok(hike);
    }

    // POST /api/hikes — create new hike
    [HttpPost]
    public async Task<IActionResult> CreateHike([FromBody] HikeCreateDto dto)
    {
        var hike = new Hike
        {
            Name = dto.Name,
            Area = dto.Area,
            DistanceKm = dto.DistanceKm,
            ElevationM = dto.ElevationM,
            DurationMinutes = dto.DurationMinutes,
            Difficulty = dto.Difficulty,
            TrackJson = dto.TrackJson,
            PhotosJson = dto.PhotosJson,
            UserId = GetUserId()
        };
        _db.Hikes.Add(hike);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetHike), new { id = hike.Id }, hike);
    }

    // DELETE /api/hikes/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHike(int id)
    {
        var hike = await _db.Hikes.FirstOrDefaultAsync(h => h.Id == id && h.UserId == GetUserId());
        if (hike == null) return NotFound();
        _db.Hikes.Remove(hike);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
