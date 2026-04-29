using EMS.API.DTOs;
using EMS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeesController(EmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<EmployeeResponseDto>>> GetEmployees([FromQuery] EmployeeQueryParams queryParams)
        {
            var result = await _service.GetAllEmployeesAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponseDto>> GetEmployee(int id)
        {
            var result = await _service.GetEmployeeByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EmployeeResponseDto>> PostEmployee([FromBody] EmployeeRequestDto dto)
        {
            try
            {
                var result = await _service.CreateEmployeeAsync(dto);
                return CreatedAtAction(nameof(GetEmployee), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EmployeeResponseDto>> PutEmployee(int id, [FromBody] EmployeeRequestDto dto)
        {
            try
            {
                var result = await _service.UpdateEmployeeAsync(id, dto);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _service.DeleteEmployeeAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("dashboard")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<ActionResult<DashboardSummaryDto>> GetDashboardSummary()
        {
            var result = await _service.GetDashboardSummaryAsync();
            return Ok(result);
        }
    }
}