using System;
using CustomerService.DAL.Models;
using CustomerService.ViewModel;

namespace CustomerService.Services.Interfaces
{
	public interface IUserService
	{
        Task<UserViewModel?> GetUserByUserName(string userName);
        Task<bool> IsValidUserName(string userName);
        Task<UserViewModel> CreateUser(UserViewModel user, string role);
        Task<bool> IsCustomerAsync(long userID);

    }
}

