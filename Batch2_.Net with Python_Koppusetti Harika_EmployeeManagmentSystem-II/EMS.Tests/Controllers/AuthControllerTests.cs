using Xunit;
using Moq;
using EMS.API.Controllers;
using EMS.API.Services;
using EMS.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EMS.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authMock = new Mock<IAuthService>();
            _controller = new AuthController(_authMock.Object);
        }

        [Fact]
        public async Task Register_Success_ReturnsOk()
        {
            _authMock.Setup(a => a.RegisterAsync(It.IsAny<RegisterRequestDto>()))
                .ReturnsAsync(new AuthResponseDto { Success = true, Token = "abc" });

            var result = await _controller.Register(new RegisterRequestDto());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Register_Fail_ReturnsBadRequest()
        {
            _authMock.Setup(a => a.RegisterAsync(It.IsAny<RegisterRequestDto>()))
                .ReturnsAsync(new AuthResponseDto { Success = false });

            var result = await _controller.Register(new RegisterRequestDto());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_Success_ReturnsOk()
        {
            _authMock.Setup(a => a.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync(new AuthResponseDto { Success = true, Token = "abc" });

            var result = await _controller.Login(new LoginRequestDto());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_Fail_ReturnsUnauthorized()
        {
            _authMock.Setup(a => a.LoginAsync(It.IsAny<LoginRequestDto>()))
                .ReturnsAsync(new AuthResponseDto { Success = false });

            var result = await _controller.Login(new LoginRequestDto());

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}