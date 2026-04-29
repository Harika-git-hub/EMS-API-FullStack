using System.ComponentModel.DataAnnotations;

namespace EMS.API.DTOs;

public class EmployeeRequestDto
{
    [Required, MaxLength(100)]
    public string? FirstName { get; set; } = string.Empty;
    
    [Required, MaxLength(100)]
    public string? LastName { get; set; } = string.Empty;
    
    [Required, EmailAddress, MaxLength(200)]
    public string? Email { get; set; } = string.Empty;
    
    [Required, RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be 10 digits")]
    public string? Phone { get; set; } = string.Empty;
    
    [Required]
    public string? Department { get; set; } = string.Empty;
    
    [Required, MaxLength(100)]
    public string? Designation { get; set; } = string.Empty;
    
    [Required, Range(0.01, double.MaxValue)]
    public decimal Salary { get; set; }
    
    [Required]
    public DateTime? JoinDate { get; set; }
    
    [Required]
    public string? Status { get; set; } = "Active";
}

public class EmployeeResponseDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? Department { get; set; } = string.Empty;
    public string? Designation { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime? JoinDate { get; set; }
    public string? Status { get; set; } = "Active";
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class EmployeeQueryParams
{
    public string? Search { get; set; }
    public string? Department { get; set; }
    public string? Status { get; set; }
    public string? SortBy { get; set; } = "name";
    public string? SortDir { get; set; } = "asc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class PagedResult<T>
{
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPrevPage => Page > 1;
}

public class DashboardSummaryDto
{
    public int TotalEmployees { get; set; }
    public int ActiveCount { get; set; }
    public int InactiveCount { get; set; }
    public int TotalDepartments { get; set; }
    public List<DepartmentBreakdown> DepartmentBreakdown { get; set; } = new();
    public List<EmployeeResponseDto> RecentEmployees { get; set; } = new();
}

public class DepartmentBreakdown
{
    public string? Department { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}