using System;
using AutoMapper;
using CustomerService.Infrastructure.Models;
using CustomerService.Infrastructure.Repositories.Interfaces;
using CustomerService.Services.Interfaces;
using CustomerService.Core.Models;

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

		public async Task<UserDto?> GetUserByUserName(string userName)
		{
			var user = await _userRepository.GetUserByUserName(userName);
			return mapper.Map<UserDto>(user);
		}
        public async Task<bool> IsValidUserName(string userName)
		{
			return await _userRepository.IsValidUserName(userName);
		}
        public async Task<bool> IsCustomerAsync(long userID)
        {
            return await _userRepository.IsCustomerAsync(userID);
        }
        public async Task<UserDto> CreateUser(UserDto user, string role)
		{
			var userModel = mapper.Map<User>(user);
			userModel = await _userRepository.CreateUser(userModel, role);
			return mapper.Map<UserDto>(userModel);
		}

    }
}

