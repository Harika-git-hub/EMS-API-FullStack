using Xunit;
using Moq;
using EMS.API.Controllers;
using EMS.API.Services;
using EMS.API.DTOs;
using EMS.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EMS.Tests.Controllers
{
    public class EmployeesControllerTests
    {
        private readonly Mock<IEmployeeRepository> _repoMock;
        private readonly EmployeeService _service;
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _repoMock = new Mock<IEmployeeRepository>();
            _service = new EmployeeService(_repoMock.Object);
            _controller = new EmployeesController(_service);
        }

        [Fact]
        public async Task GetEmployee_Valid_ReturnsOk()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Employee { Id = 1, FirstName = "A", LastName = "B" });

            var result = await _controller.GetEmployee(1);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetEmployee_Invalid_ReturnsNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Employee)null);

            var result = await _controller.GetEmployee(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostEmployee_Valid_ReturnsCreated()
        {
            _repoMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), null))
                .ReturnsAsync(false);

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Employee>()))
                .ReturnsAsync((Employee e) => e);

            var dto = new EmployeeRequestDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "test@gmail.com",
                Phone = "1234567890",
                Salary = 1000,
                JoinDate = System.DateTime.UtcNow.AddDays(-1)
            };

            var result = await _controller.PostEmployee(dto);

            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task PostEmployee_Invalid_ReturnsBadRequest()
        {
            var dto = new EmployeeRequestDto(); // invalid

            var result = await _controller.PostEmployee(dto);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task DeleteEmployee_Valid_ReturnsNoContent()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Employee());

            var result = await _controller.DeleteEmployee(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_Invalid_ReturnsNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Employee)null);

            var result = await _controller.DeleteEmployee(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}