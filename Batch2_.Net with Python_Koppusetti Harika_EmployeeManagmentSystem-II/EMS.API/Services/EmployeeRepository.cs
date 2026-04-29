using Microsoft.EntityFrameworkCore;
using EMS.API.Data;
using EMS.API.Models;
using EMS.API.DTOs;
using EMS.API.Services;

namespace EMS.API.Services;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees.FindAsync(id);
    }

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
    {
        return await _context.Employees.AnyAsync(e => e.Email == email && (excludeId == null || e.Id != excludeId));
    }

    public async Task<PagedResult<EmployeeResponseDto>> GetAllAsync(EmployeeQueryParams queryParams)
    {
        var query = _context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParams.Search))
        {
            var search = queryParams.Search.ToLower();
            query = query.Where(e => 
                (e.FirstName ?? "").ToLower().Contains(search) || 
                (e.LastName ?? "").ToLower().Contains(search) || 
                (e.Email ?? "").ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(queryParams.Department))
        {
            query = query.Where(e => e.Department == queryParams.Department);
        }

        if (!string.IsNullOrWhiteSpace(queryParams.Status))
        {
            query = query.Where(e => e.Status == queryParams.Status);
        }

        var sortBy = queryParams.SortBy?.ToLower() ?? "firstname";
        var sortDir = queryParams.SortDir?.ToLower() ?? "asc";

        query = sortBy switch
        {
            "department" => sortDir == "desc" 
                ? query.OrderByDescending(e => e.Department) 
                : query.OrderBy(e => e.Department),
            "salary" => sortDir == "desc" 
                ? query.OrderByDescending(e => e.Salary) 
                : query.OrderBy(e => e.Salary),
            _ => sortDir == "desc" 
                ? query.OrderByDescending(e => e.FirstName) 
                : query.OrderBy(e => e.FirstName)
        };

        var totalCount = await query.CountAsync();

        var employees = await query
            .Skip((queryParams.Page - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Select(e => new EmployeeResponseDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                Department = e.Department,
                Designation = e.Designation,
                Salary = e.Salary,
                JoinDate = e.JoinDate,
                Status = e.Status,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            })
            .ToListAsync();

        return new PagedResult<EmployeeResponseDto>
        {
            Data = employees,
            TotalCount = totalCount,
            Page = queryParams.Page,
            PageSize = queryParams.PageSize
        };
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        var totalEmployees = await _context.Employees.CountAsync();
        var activeCount = await _context.Employees.CountAsync(e => e.Status == "Active");
        var inactiveCount = totalEmployees - activeCount;
        var totalDepartments = await _context.Employees.Select(e => e.Department).Distinct().CountAsync();

        var departmentGroups = await _context.Employees
            .GroupBy(e => e.Department)
            .Select(g => new DepartmentBreakdown
            {
                Department = g.Key,
                Count = g.Count(),
                Percentage = totalEmployees > 0 ? Math.Round((double)g.Count() / totalEmployees * 100, 2) : 0
            })
            .ToListAsync();

        var recentEmployees = await _context.Employees
            .OrderByDescending(e => e.CreatedAt)
            .Take(5)
            .Select(e => new EmployeeResponseDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                Department = e.Department,
                Designation = e.Designation,
                Salary = e.Salary,
                JoinDate = e.JoinDate,
                Status = e.Status,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            })
            .ToListAsync();

        return new DashboardSummaryDto
        {
            TotalEmployees = totalEmployees,
            ActiveCount = activeCount,
            InactiveCount = inactiveCount,
            TotalDepartments = totalDepartments,
            DepartmentBreakdown = departmentGroups,
            RecentEmployees = recentEmployees
        };
    }
}