namespace TimeDotLog.Data.Models.Dtos;

public record ReportRequest(string Title, string Content);
public record ReportDetailDto(int Id, int AttendanceId, string Title, string Content, DateTime SubmittedAt);