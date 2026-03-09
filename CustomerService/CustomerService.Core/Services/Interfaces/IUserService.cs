using System;
using CustomerService.Infrastructure.Models;
using CustomerService.Core.Models;

namespace CustomerService.Services.Interfaces
{
	public interface IUserService
	{
        Task<UserDto?> GetUserByUserName(string userName);
        Task<bool> IsValidUserName(string userName);
        Task<UserDto> CreateUser(UserDto user, string role);
        Task<bool> IsCustomerAsync(long userID);

    }
}

