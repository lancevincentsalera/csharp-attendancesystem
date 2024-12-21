namespace TimeDotLog.Data.Models.Dtos;

public record EmployeeAttendanceDisplayDto(string FirstName, string TimeIn, string? TimeOut);
public record EmployeeDetailDto(int Id, string Username, string FirstName, List<AttendanceDetailDto> Attendances);
public record UpdateEmployeeRequest(string Username, string FirstName, string Password, string DiscordAuthToken);