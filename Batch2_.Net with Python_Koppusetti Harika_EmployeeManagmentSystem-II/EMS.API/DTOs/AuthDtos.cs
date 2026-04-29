using System.ComponentModel.DataAnnotations;

namespace EMS.API.DTOs;

public class RegisterRequestDto
{
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
    public string Role { get; set; } = "Viewer";
}

public class LoginRequestDto
{
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Role { get; set; }
    public string? Token { get; set; }
}