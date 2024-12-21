

using Microsoft.EntityFrameworkCore;
using TimeDotLog.Data;
using TimeDotLog.Data.Models.Dtos;

namespace TimeDotLog.Endpoints;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this WebApplication app)
    {
        app.MapGet("/api/employees", GetAllEmployees).WithName("GetAllEmployees").WithTags("Employees");
        app.MapGet("/api/employees/{id}", GetEmployeeById).WithName("GetEmployeeById").WithTags("Employees");
        app.MapGet("/api/employees/{employeeId}/discordAuth", GetDiscordAuthToken).WithName("GetDiscordAuthToken").WithTags("Employees");
        app.MapDelete("/api/employees/{employeeId}/delete", DeleteEmployee).WithName("DeleteEmployee").WithTags("Employees");
        app.MapPut("/api/employees/{employeeId}/update", UpdateEmployee).WithName("UpdateEmployee").WithTags("Employees");
    }


    public static async Task<IResult> GetAllEmployees(TimeDotLogContext context)
    {
        var employees = await context.Employees
            .Select(e => new EmployeeDetailDto(
                e.Id,
                e.Username,
                e.FirstName,
                e.Attendances
                    .Select(a => new AttendanceDetailDto(a.Id, a.EmployeeId, a.TimeIn, a.TimeOut))
                    .ToList()))
            .ToListAsync();

        return Results.Ok(employees);
    }


    public static async Task<IResult> GetEmployeeById(TimeDotLogContext context, int id)
    {
        var employee = await context.Employees
            .Include(e => e.Attendances)
            .Where(e => e.Id == id)
            .Select(e => new EmployeeDetailDto(
                e.Id,
                e.Username,
                e.FirstName,
                e.Attendances
                    .Select(a => new AttendanceDetailDto(a.Id, a.EmployeeId, a.TimeIn, a.TimeOut))
                    .ToList()))
            .FirstOrDefaultAsync();

        if (employee is null) return Results.NotFound();

        return Results.Ok(employee);
    }

    public static async Task<IResult> GetDiscordAuthToken(TimeDotLogContext context, int employeeId)
    {
        var employee = await context.Employees.FindAsync(employeeId);
        if (employee == null) return Results.NotFound();
        return Results.Ok(employee.DiscordAuthToken);
    }


    public static async Task<IResult> DeleteEmployee(TimeDotLogContext context, int employeeId)
    {
        var employee = await context.Employees.FindAsync(employeeId);
        if (employee == null) return Results.NotFound();
        context.Employees.Remove(employee);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    public static async Task<IResult> UpdateEmployee(TimeDotLogContext context, int employeeId, UpdateEmployeeRequest request)
    {
        var employee = await context.Employees.FindAsync(employeeId);
        if (employee == null) return Results.NotFound();
        employee.Username = request.Username;
        employee.FirstName = request.FirstName;
        employee.Password = request.Password;
        employee.DiscordAuthToken = request.DiscordAuthToken;
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}