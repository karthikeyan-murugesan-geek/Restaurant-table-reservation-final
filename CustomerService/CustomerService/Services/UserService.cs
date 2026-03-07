using System;
using AutoMapper;
using CustomerService.DAL.Models;
using CustomerService.DAL.Repositories.Interfaces;
using CustomerService.Services.Interfaces;
using CustomerService.ViewModel;

namespace CustomerService.Services
{
	public class UserService :IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper mapper;
		public UserService(IUserRepository userRepositpory, IMapper mapper)
		{
			this._userRepository = userRepositpory;
			this.mapper = mapper;
		}

		public async Task<UserViewModel?> GetUserByUserName(string userName)
		{
			var user = await _userRepository.GetUserByUserName(userName);
			return mapper.Map<UserViewModel>(user);
		}
        public async Task<bool> IsValidUserName(string userName)
		{
			return await _userRepository.IsValidUserName(userName);
		}
        public async Task<bool> IsCustomerAsync(long userID)
        {
            return await _userRepository.IsCustomerAsync(userID);
        }
        public async Task<UserViewModel> CreateUser(UserViewModel user, string role)
		{
			var userModel = mapper.Map<User>(user);
			userModel = await _userRepository.CreateUser(userModel, role);
			return mapper.Map<UserViewModel>(userModel);
		}

    }
}

