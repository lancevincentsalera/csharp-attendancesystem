namespace TimeDotLog.Data.Models.Dtos;

public record RegisterRequest(string Username, string FirstName, string Password, string DiscordAuthToken);
public record LoginRequest(string Username, string Password);