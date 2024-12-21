using Microsoft.EntityFrameworkCore;
using TimeDotLog.Data;
using TimeDotLog.Data.Models;
using TimeDotLog.Data.Models.Dtos;

namespace TimeDotLog.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/register", RegisterUser).WithName("RegisterUser").WithTags("Auth");
        app.MapPost("/api/login", LoginUser).WithName("LoginUser").WithTags("Auth");
    }


    public static async Task<IResult> RegisterUser(TimeDotLogContext context, RegisterRequest request)
    {
        try
        {
            var employee = new Employee
            {
                Username = request.Username,
                FirstName = request.FirstName,
                Password = request.Password,
                DiscordAuthToken = request.DiscordAuthToken
            };
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
            var employeeDto = new EmployeeDetailDto(
                employee.Id,
                employee.Username,
                employee.FirstName,
                employee.Attendances
                    .Select(a => new AttendanceDetailDto(a.Id, a.EmployeeId, a.TimeIn, a.TimeOut))
                    .ToList());
            return Results.Created($"/api/employees/{employee.Id}", employeeDto);
        }
        catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Results.BadRequest("Username already exists");
        }
    }

    public static async Task<IResult> LoginUser(TimeDotLogContext context, LoginRequest request)
    {
        var employee = await context.Employees.FirstOrDefaultAsync(e => e.Username == request.Username && e.Password == request.Password);
        if (employee == null) return Results.NotFound();
        return Results.Ok("Login successful");
    }
}