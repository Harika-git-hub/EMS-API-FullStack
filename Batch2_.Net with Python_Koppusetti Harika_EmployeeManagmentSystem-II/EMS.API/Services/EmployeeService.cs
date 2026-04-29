using EMS.API.DTOs;
using EMS.API.Services;
using EMS.API.Models;
using System.Text.RegularExpressions;

namespace EMS.API.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EmployeeResponseDto>> GetAllEmployeesAsync(EmployeeQueryParams queryParams)
    {
        return await _repository.GetAllAsync(queryParams);
    }

    public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee == null) return null;
        
        return MapToDto(employee);
    }

    public async Task<EmployeeResponseDto> CreateEmployeeAsync(EmployeeRequestDto request)
    {
        ValidateEmployee(request);

        if (await _repository.ExistsByEmailAsync(request.Email!))
            throw new InvalidOperationException("Email already exists");

        var employee = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            Department = request.Department,
            Designation = request.Designation,
            Salary = request.Salary,
            JoinDate = request.JoinDate,
            Status = request.Status,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(employee);
        return MapToDto(created);
    }

    public async Task<EmployeeResponseDto?> UpdateEmployeeAsync(int id, EmployeeRequestDto request)
    {
        ValidateEmployee(request);

        var employee = await _repository.GetByIdAsync(id);
        if (employee == null) return null;

        if (await _repository.ExistsByEmailAsync(request.Email!, id))
            throw new InvalidOperationException("Email already exists");

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Email = request.Email;
        employee.Phone = request.Phone;
        employee.Department = request.Department;
        employee.Designation = request.Designation;
        employee.Salary = request.Salary;
        employee.JoinDate = request.JoinDate;
        employee.Status = request.Status;
        employee.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(employee);
        return MapToDto(employee);
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);
        if (employee == null) return false;

        await _repository.DeleteAsync(employee);
        return true;
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        return await _repository.GetDashboardSummaryAsync();
    }

    private void ValidateEmployee(EmployeeRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) || request.FirstName.Length < 2)
            throw new ArgumentException("FirstName must be at least 2 characters");

        if (string.IsNullOrWhiteSpace(request.LastName) || request.LastName.Length < 2)
            throw new ArgumentException("LastName must be at least 2 characters");

        if (string.IsNullOrWhiteSpace(request.Email) || !Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email format");

        if (string.IsNullOrWhiteSpace(request.Phone) || !Regex.IsMatch(request.Phone, @"^\d{10}$"))
            throw new ArgumentException("Phone must be 10 digits");

        if (request.Salary <= 0)
            throw new ArgumentException("Salary must be greater than 0");

        if (request.JoinDate > DateTime.UtcNow.Date)
            throw new ArgumentException("JoinDate cannot be in the future");
    }

    private EmployeeResponseDto MapToDto(Employee employee)
    {
        return new EmployeeResponseDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            Department = employee.Department,
            Designation = employee.Designation,
            Salary = employee.Salary,
            JoinDate = employee.JoinDate,
            Status = employee.Status,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }
}