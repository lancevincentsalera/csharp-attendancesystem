using Microsoft.EntityFrameworkCore;
using TimeDotLog.Data.Models;

namespace TimeDotLog.Data;

public class TimeDotLogContext(DbContextOptions<TimeDotLogContext> options) : DbContext(options)
{
    public DbSet<Attendance> Attendances { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Report> Reports { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Employee)
            .WithMany(e => e.Attendances)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Report>()
            .HasOne(r => r.Attendance)
            .WithOne(a => a.Report)
            .HasForeignKey<Report>(r => r.AttendanceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Username)
            .IsUnique();
    }
}