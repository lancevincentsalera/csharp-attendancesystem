namespace TimeDotLog.Data.Models.Dtos;

public record AttendanceDetailDto(int Id, int EmployeeId, DateTime TimeIn, DateTime? TimeOut);