using System.ComponentModel.DataAnnotations;

namespace TimeDotLog.Data.Models;

public class Attendance
{
    [Key]
    public int Id { get; set; }
    public DateTime TimeIn { get; set; }
    public DateTime? TimeOut { get; set; }
    public int EmployeeId { get; set; }
    public virtual Employee Employee { get; set; } = null!;
    public virtual Report? Report { get; set; }
}