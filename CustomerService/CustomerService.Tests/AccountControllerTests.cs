using CustomerService.API.Controllers;
using CustomerService.Models;
using CustomerService.Core.Services.Interfaces;
using CustomerService.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CustomerService.Tests;
public class AccountControllerTests
{
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly AppSettings _appSettings = new AppSettings
    {
         Jwt=new Jwt { Key = "ThisIsASecretKeyForTestingPurposes12345" }
    };

    private AccountController GetController()
    {
        return new AccountController(_userServiceMock.Object, _appSettings, _tokenService.Object, _authServiceMock.Object);
    }

    [Fact]
    public async Task SignUp_UserNameExists_ReturnsBadRequest()
    {
        // Arrange
        var controller = GetController();
        var signup = new SignupDto { UserName = "John", Password = "Pass123", Role = Core.Enums.UserRole.Customer, MobileNumber = "1234567890" };
        _userServiceMock.Setup(s => s.CreateUser(It.IsAny<SignupDto>()))
            .ReturnsAsync((UserDto?)null);
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
        var signup = new SignupDto { UserName = "James", Password = "Pass123", Role = Core.Enums.UserRole.Customer, MobileNumber = "1234567890" };

        _userServiceMock.Setup(s => s.IsValidUserName("John"))
            .ReturnsAsync(false);

        _userServiceMock.Setup(s => s.CreateUser(It.IsAny<SignupDto>()))
            .ReturnsAsync(new UserDto { UserID = 1, UserName = "John" });

        // Act
        var result = await controller.SignUp(signup);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"User created as {signup.Role} John", okResult.Value);
    }

    [Fact]
    public async Task Login_UserNotFound_InvalidPwd_ReturnsUnauthorized()
    {
        // Arrange
        var controller = GetController();
        var login = new LoginDto { UserName = "John", Password = "pass" };

        _authServiceMock.Setup(s => s.LoginAsync(login))
            .ReturnsAsync(new LoginResultDto { Success = false});

        // Act
        var result = await controller.Login(login);

        // Assert
        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("User not found", unauthorized.Value);
    }

    [Fact]
    public async Task Login_ValidUser_ReturnsTokens()
    {
        // Arrange
        var controller = GetController();
        var login = new LoginDto { UserName = "John", Password = "pass1" };

        var loginResult = new LoginResultDto
        {
            Success = true,
            AccessToken = "tempToken",
            RefreshToken = "tempToken"
        };

        _authServiceMock.Setup(s => s.LoginAsync(login))
            .ReturnsAsync(loginResult);

        // Act
        var result = await controller.Login(login);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
    }
}

