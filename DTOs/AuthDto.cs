namespace TrailFrame.API.DTOs;

public record RegisterDto(string Username, string Email, string Password);

public record LoginDto(string Email, string Password);

public record AuthResponseDto(string Token, string Username, int UserId);

public record HikeCreateDto(
    string Name,
    string Area,
    double DistanceKm,
    int ElevationM,
    int DurationMinutes,
    string Difficulty,
    string TrackJson,
    string PhotosJson
);
