using System;
using CustomerService.Core.Helpers;
using CustomerService.Core.Models;
using CustomerService.Core.Services;
using CustomerService.Core.Services.Interfaces;
using CustomerService.Infrastructure.Models;
using CustomerService.Models;
using Moq;
using Xunit;

namespace CustomerService.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<AppSettings> _appSettings;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _appSettings = new Mock<AppSettings>();
            _authService = new AuthService(
                _userServiceMock.Object,
                _tokenServiceMock.Object,
                _appSettings.Object
            );
        }

        [Fact]
        public async Task LoginAsync_UserNotFound_ReturnsFailure()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                UserName = "test",
                Password = "password"
            };

            _userServiceMock
                .Setup(x => x.GetUserByUserName(loginDto.UserName))
                .ReturnsAsync((UserDto)null);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User not found", result.Message);
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_ReturnsFailure()
        {
            // Arrange
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("wrongpwd");

            var user = new UserDto
            {
                UserID = 1,
                UserName = "test",
                PasswordHash = passwordHash,
                Roles = new List<string> { "User" }
            };

            var loginDto = new LoginDto
            {
                UserName = "test",
                Password = "pwd"
            };

            _userServiceMock
                .Setup(x => x.GetUserByUserName(loginDto.UserName))
                .ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid password", result.Message);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsTokens()
        {
            // Arrange
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("password123");

            var user = new UserDto
            {
                UserID = 1,
                UserName = "test",
                PasswordHash = passwordHash,
                Roles = new List<string> { "Admin" }
            };

            var loginDto = new LoginDto
            {
                UserName = "test",
                Password = "password123"
            };

            _userServiceMock
                .Setup(x => x.GetUserByUserName(loginDto.UserName))
                .ReturnsAsync(user);

            _tokenServiceMock
                .Setup(x => x.GenereteToken(It.IsAny<TokenRequestDto>()))
                .Returns("temptoken");

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("temptoken", result.AccessToken);
            Assert.Equal("temptoken", result.RefreshToken);
        }
    }
}

