using EMS.API.DTOs;
using EMS.API.Models;

namespace EMS.API.Services;

public interface IEmployeeRepository
{
    Task<PagedResult<EmployeeResponseDto>> GetAllAsync(EmployeeQueryParams queryParams);
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
    Task<DashboardSummaryDto> GetDashboardSummaryAsync();
    Task<bool> ExistsByEmailAsync(string email, int? excludeId = null); // Add this
}