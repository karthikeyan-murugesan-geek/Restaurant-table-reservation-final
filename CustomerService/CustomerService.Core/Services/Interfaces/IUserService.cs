using System;
using CustomerService.Infrastructure.Models;
using CustomerService.Core.Models;

namespace CustomerService.Core.Services.Interfaces
{
	public interface IUserService
	{
        Task<UserDto?> GetUserByUserName(string userName);
        Task<bool> IsValidUserName(string userName);
        Task<UserDto?> CreateUser(SignupDto user, string role);
        Task<bool> IsCustomerAsync(long userID);

    }
}

