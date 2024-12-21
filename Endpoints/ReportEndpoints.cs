using Microsoft.EntityFrameworkCore;
using TimeDotLog.Data;
using TimeDotLog.Data.Models;
using TimeDotLog.Data.Models.Dtos;

namespace TimeDotLog.Endpoints;

public static class ReportEndpoints
{
    public static void MapReportEndpoints(this WebApplication app)
    {
        app.MapGet("/api/reports", GetAllReports).WithName("GetAllReports").WithTags("Reports");
        app.MapGet("/api/reports/{id}", GetReportById).WithName("GetReportById").WithTags("Reports");
        app.MapPost("/api/reports/create/{attendanceId}", CreateReport).WithName("CreateReport").WithTags("Reports");
    }

    public static async Task<IResult> GetAllReports(TimeDotLogContext context)
    {
        var reports = await context.Reports
            .Select(r => new ReportDetailDto(r.Id, r.AttendanceId, r.Title, r.Content, r.SubmittedAt))
            .ToListAsync();

        return Results.Ok(reports);
    }

    public static async Task<IResult> GetReportById(TimeDotLogContext context, int id)
    {
        var report = await context.Reports
            .Where(r => r.Id == id)
            .Select(r => new ReportDetailDto(r.Id, r.AttendanceId, r.Title, r.Content, r.SubmittedAt))
            .FirstOrDefaultAsync();

        if (report is null) return Results.NotFound();

        return Results.Ok(report);
    }

    public static async Task<IResult> CreateReport(TimeDotLogContext context, ReportRequest request, int attendanceId)
    {
        var attendance = await context.Attendances.Where(a => a.Id == attendanceId).Include(a => a.Report).FirstOrDefaultAsync();
        if (attendance is null) return Results.NotFound();
        if (attendance.Report is not null) return Results.BadRequest("You have already submitted a report for this attendance");
        var report = new Report
        {
            Title = request.Title,
            Content = request.Content,
            AttendanceId = attendanceId
        };
        await context.Reports.AddAsync(report);
        await context.SaveChangesAsync();
        var reportDto = new ReportDetailDto(report.Id, report.AttendanceId, report.Title, report.Content, report.SubmittedAt);
        return Results.Created($"/api/reports/{report.Id}", reportDto);
    }
}