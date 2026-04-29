using EMS.API.DTOs;
using EMS.API.Models;

namespace EMS.API.Services;


public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
}