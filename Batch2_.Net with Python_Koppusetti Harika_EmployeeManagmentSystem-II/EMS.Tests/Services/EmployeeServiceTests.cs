using Xunit;
using Moq;
using EMS.API.Services;
using EMS.API.Models;
using EMS.API.DTOs;
using System;
using System.Threading.Tasks;

namespace EMS.Tests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _repoMock;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _repoMock = new Mock<IEmployeeRepository>();
            _service = new EmployeeService(_repoMock.Object);
        }

        private EmployeeRequestDto GetValidDto() => new EmployeeRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@gmail.com",
            Phone = "1234567890",
            Department = "IT",
            Designation = "Dev",
            Salary = 10000,
            JoinDate = DateTime.UtcNow.AddDays(-1),
            Status = "Active"
        };

        [Fact]
        public async Task CreateEmployee_Valid_ReturnsEmployee()
        {
            var dto = GetValidDto();

            _repoMock.Setup(r => r.ExistsByEmailAsync(dto.Email!, null)).ReturnsAsync(false);
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Employee>()))
                     .ReturnsAsync((Employee e) => e);

            var result = await _service.CreateEmployeeAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Email, result.Email);
        }

        [Fact]
        public async Task CreateEmployee_EmailExists_ThrowsException()
        {
            var dto = GetValidDto();

            _repoMock.Setup(r => r.ExistsByEmailAsync(dto.Email!, null)).ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateEmployeeAsync(dto));
        }

        [Fact]
        public async Task CreateEmployee_InvalidEmail_ThrowsException()
        {
            var dto = GetValidDto();
            dto.Email = "wrong";

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateEmployeeAsync(dto));
        }

        [Fact]
        public async Task GetEmployeeById_Valid_ReturnsEmployee()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Employee { Id = 1, FirstName = "A", LastName = "B" });

            var result = await _service.GetEmployeeByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetEmployeeById_Invalid_ReturnsNull()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Employee)null);

            var result = await _service.GetEmployeeByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateEmployee_Valid_ReturnsUpdated()
        {
            var dto = GetValidDto();

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Employee { Id = 1 });

            _repoMock.Setup(r => r.ExistsByEmailAsync(dto.Email!, 1))
                .ReturnsAsync(false);

            var result = await _service.UpdateEmployeeAsync(1, dto);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateEmployee_NotFound_ReturnsNull()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Employee)null);

            var result = await _service.UpdateEmployeeAsync(1, GetValidDto());

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateEmployee_EmailExists_ThrowsException()
        {
            var dto = GetValidDto();

            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Employee { Id = 1 });

            _repoMock.Setup(r => r.ExistsByEmailAsync(dto.Email!, 1))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdateEmployeeAsync(1, dto));
        }

        [Fact]
        public async Task DeleteEmployee_Valid_ReturnsTrue()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Employee());

            var result = await _service.DeleteEmployeeAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteEmployee_NotFound_ReturnsFalse()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Employee)null);

            var result = await _service.DeleteEmployeeAsync(1);

            Assert.False(result);
        }

        [Fact]
        public async Task Validation_FirstName_Invalid()
        {
            var dto = GetValidDto();
            dto.FirstName = "A";

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateEmployeeAsync(dto));
        }

        [Fact]
        public async Task Validation_Phone_Invalid()
        {
            var dto = GetValidDto();
            dto.Phone = "123";

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateEmployeeAsync(dto));
        }

        [Fact]
        public async Task Validation_Salary_Invalid()
        {
            var dto = GetValidDto();
            dto.Salary = 0;

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateEmployeeAsync(dto));
        }
    }
}