using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TimeDotLog.Data.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(20)]
    public string Username { get; set; } = null!;
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string DiscordAuthToken { get; set; } = null!;
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
