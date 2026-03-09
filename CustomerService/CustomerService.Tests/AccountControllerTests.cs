using CustomerService.API.Controllers;
using CustomerService.Models;
using CustomerService.Services.Interfaces;
using CustomerService.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CustomerService.Tests;
public class AccountControllerTests
{
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly AppSettings _appSettings = new AppSettings
    {
         Jwt=new Jwt { Key = "ThisIsASecretKeyForTestingPurposes12345" }
    };

    private AccountController GetController()
    {
        return new AccountController(_userServiceMock.Object, _appSettings);
    }

    [Fact]
    public async Task SignUp_UserNameExists_ReturnsBadRequest()
    {
        // Arrange
        var controller = GetController();
        var signup = new SignupDto { UserName = "John", Password = "Pass123", Role = "Customer", MobileNumebr = "1234567890" };

        _userServiceMock.Setup(s => s.IsValidUserName("John"))
            .ReturnsAsync(true); 

        // Act
        var result = await controller.SignUp(signup);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username already exists", badRequest.Value);
    }

    [Fact]
    public async Task SignUp_NewUser_ReturnsOk()
    {
        // Arrange
        var controller = GetController();
        var signup = new SignupDto { UserName = "James", Password = "Pass123", Role = "Customer", MobileNumebr = "1234567890" };

        _userServiceMock.Setup(s => s.IsValidUserName("John"))
            .ReturnsAsync(false);

        _userServiceMock.Setup(s => s.CreateUser(It.IsAny<UserDto>(), "Customer"))
            .ReturnsAsync(new UserDto { UserID = 1, UserName = "John" });

        // Act
        var result = await controller.SignUp(signup);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"User created as {signup.Role} John", okResult.Value);
    }

    [Fact]
    public async Task Login_UserNotFound_ReturnsUnauthorized()
    {
        // Arrange
        var controller = GetController();
        var login = new LoginDto { UserName = "John", Password = "pass" };

        _userServiceMock.Setup(s => s.GetUserByUserName("John"))
            .ReturnsAsync((UserDto?)null);

        // Act
        var result = await controller.Login(login);

        // Assert
        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("User not found", unauthorized.Value);
    }

    [Fact]
    public async Task Login_InvalidPassword_ReturnsUnauthorized()
    {
        // Arrange
        var controller = GetController();
        var login = new LoginDto { UserName = "John", Password = "pass1" };

        var user = new UserDto
        {
            UserID = 1,
            UserName = "John",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass2"),
            Roles = new List<string>() { "Customer" }
        };

        _userServiceMock.Setup(s => s.GetUserByUserName("John"))
            .ReturnsAsync(user);

        // Act
        var result = await controller.Login(login);

        // Assert
        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid password", unauthorized.Value);
    }

    [Fact]
    public async Task Login_ValidUser_ReturnsTokens()
    {
        // Arrange
        var controller = GetController();
        var login = new LoginDto { UserName = "John", Password = "pass1" };

        var user = new UserDto
        {
            UserID = 1,
            UserName = "John",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass1"),
            Roles = new List<string>() { "Customer" }
        };

        _userServiceMock.Setup(s => s.GetUserByUserName("John"))
            .ReturnsAsync(user);

        // Act
        var result = await controller.Login(login);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
    }
}

