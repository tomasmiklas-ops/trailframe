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
    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetMyHikes()
    {
        var userId = GetUserId();
        var hikes = await _db.Hikes
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.Date)
            .Select(h => new { h.Id, h.Name, h.Area, h.DistanceKm, h.ElevationM, h.DurationMinutes, h.Difficulty, h.Date, h.TrackJson, h.PhotosJson })
            .ToListAsync();
        return Ok(hikes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHike(int id)
    {
        var hike = await _db.Hikes.FirstOrDefaultAsync(h => h.Id == id && h.UserId == GetUserId());
        if (hike == null) return NotFound();
        return Ok(hike);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHike([FromBody] HikeCreateDto dto)
    {
        var hike = new Hike
        {
            Name = dto.Name, Area = dto.Area, DistanceKm = dto.DistanceKm,
            ElevationM = dto.ElevationM, DurationMinutes = dto.DurationMinutes,
            Difficulty = dto.Difficulty, TrackJson = dto.TrackJson,
            PhotosJson = dto.PhotosJson, UserId = GetUserId()
        };
        _db.Hikes.Add(hike);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetHike), new { id = hike.Id }, hike);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHike(int id, [FromBody] System.Text.Json.JsonElement body)
    {
        var hike = await _db.Hikes.FirstOrDefaultAsync(h => h.Id == id && h.UserId == GetUserId());
        if (hike == null) return NotFound();
        if (body.TryGetProperty("distanceKm", out var dist)) hike.DistanceKm = dist.GetDouble();
        if (body.TryGetProperty("durationMinutes", out var dur)) hike.DurationMinutes = dur.GetInt32();
        if (body.TryGetProperty("trackJson", out var track)) hike.TrackJson = track.GetString()!;
        await _db.SaveChangesAsync();
        return Ok(hike);
    }

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
