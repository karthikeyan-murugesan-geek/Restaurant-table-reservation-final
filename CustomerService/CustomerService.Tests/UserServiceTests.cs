using System;
using AutoMapper;
using CustomerService.Core.Models;
using CustomerService.Core.Services;
using CustomerService.Infrastructure.Models;
using CustomerService.Infrastructure.Repositories.Interfaces;
using Moq;
using Xunit;

namespace CustomerService.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task CreateUser_UserNameExists_ReturnsNull()
        {
            
            var signup = new SignupDto
            {
                UserName = "test",
                Password = "password",
                MobileNumber = "9999999999"
            };

            _userRepositoryMock.Setup(x => x.IsValidUserName(signup.UserName))
                   .ReturnsAsync(true);

            
            var result = await _userService.CreateUser(signup, "User");

            
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUser_ValidUser_ReturnsUserDto()
        {
            // Arrange
            var signup = new SignupDto
            {
                UserName = "test",
                Password = "password",
                MobileNumber = "9999999999"
            };

            var userDto = new UserDto
            {
                UserName = signup.UserName,
                MobileNumber = signup.MobileNumber
            };

            var userEntity = new User
            {
                UserName = signup.UserName,
                MobileNumber = signup.MobileNumber
            };

            _mapperMock.Setup(x => x.Map<User>(It.IsAny<UserDto>()))
                       .Returns(userEntity);

            _mapperMock.Setup(x => x.Map<UserDto>(It.IsAny<User>()))
                       .Returns(userDto);

            _userRepositoryMock
                .Setup(x => x.CreateUser(It.IsAny<User>(), "User"))
                .ReturnsAsync(userEntity);

            _userRepositoryMock.Setup(x => x.IsValidUserName(signup.UserName))
                   .ReturnsAsync(false);

            // Act
            var result = await _userService.CreateUser(signup, "User");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test", result.UserName);
        }
    }
}

