using System;
using CustomerService.DAL.Models;

namespace CustomerService.DAL.Repositories.Interfaces
{
	public interface IUserRepository
	{
        Task<User?> GetUserByUserName(string userName);
        Task<bool> IsValidUserName(string userName);
        Task<User> CreateUser(User user, string role);
        Task<bool> IsCustomerAsync(long userID);
    }
}

