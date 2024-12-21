using Microsoft.EntityFrameworkCore;
using TimeDotLog.Data;
using TimeDotLog.Data.Models;
using TimeDotLog.Data.Models.Dtos;

namespace TimeDotLog.Endpoints;

public static class AttendanceEndpoints
{
    public static void MapAttendanceEndpoints(this WebApplication app)
    {
        app.MapGet("/api/attendances", GetAllAttendances).WithName("GetAllAttendances").WithTags("Attendances");
        app.MapGet("/api/attendances/{id}", GetAttendanceById).WithName("GetAttendanceById").WithTags("Attendances");
        app.MapPost("/api/attendance/{employeeId}/timeIn", EmployeeTimeIn).WithName("EmployeeTimeIn").WithTags("Attendances");
        app.MapPut("/api/attendance/{employeeId}/timeOut", EmployeeTimeOut).WithName("EmployeeTimeOut").WithTags("Attendances");
    }

    public static async Task<IResult> GetAllAttendances(TimeDotLogContext context)
    {
        var attendances = await context.Attendances
            .Select(a => new AttendanceDetailDto(a.Id, a.EmployeeId, a.TimeIn, a.TimeOut))
            .ToListAsync();

        return Results.Ok(attendances);
    }

    public static async Task<IResult> GetAttendanceById(TimeDotLogContext context, int id)
    {
        var attendance = await context.Attendances
            .Where(a => a.Id == id)
            .Select(a => new AttendanceDetailDto(a.Id, a.EmployeeId, a.TimeIn, a.TimeOut))
            .FirstOrDefaultAsync();

        if (attendance is null) return Results.NotFound();

        return Results.Ok(attendance);
    }


    public static async Task<IResult> EmployeeTimeIn(TimeDotLogContext context, int employeeId)
    {
        var employee = await context.Employees.Include(e => e.Attendances).FirstOrDefaultAsync(e => e.Id == employeeId);
        if (employee == null) return Results.NotFound();
        var now = DateTime.UtcNow;
        if (employee.Attendances.Any(a => a.TimeIn.Date == now.Date && a.TimeOut is null)) return Results.BadRequest("You have already clocked in today");
        var attendance = new Attendance
        {
            TimeIn = DateTime.UtcNow,
            EmployeeId = employeeId
        };
        await context.Attendances.AddAsync(attendance);
        await context.SaveChangesAsync();
        var attendanceDto = new AttendanceDetailDto(attendance.Id, attendance.EmployeeId, attendance.TimeIn, attendance.TimeOut);
        return Results.Created($"/api/employees/attendance/{attendance.Id}", attendanceDto);
    }


    public static async Task<IResult> EmployeeTimeOut(TimeDotLogContext context, int employeeId)
    {
        var employee = await context.Employees.Include(e => e.Attendances).FirstOrDefaultAsync(e => e.Id == employeeId);
        if (employee == null) return Results.NotFound();
        var attendance = employee.Attendances.FirstOrDefault(a => a.TimeOut == null);
        if (attendance == null) return Results.NotFound();
        attendance.TimeOut = DateTime.Now;
        await context.SaveChangesAsync();
        return Results.Ok();
    }
}