using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeDotLog.Data.Models;

public class Report
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Title { get; set; } = null!;
    [Required]
    public string Content { get; set; } = null!;
    [Required]
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public int AttendanceId { get; set; }
    public virtual Attendance Attendance { get; set; } = null!;
}