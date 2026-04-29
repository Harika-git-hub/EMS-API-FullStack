using System.ComponentModel.DataAnnotations;

namespace EMS.API.Models;
public class AppUser
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Role { get; set; } = "Viewer"; // Admin or Viewer

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}