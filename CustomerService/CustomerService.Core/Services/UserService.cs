using System;
using AutoMapper;
using CustomerService.Infrastructure.Models;
using CustomerService.Infrastructure.Repositories.Interfaces;
using CustomerService.Core.Services.Interfaces;
using CustomerService.Core.Models;

namespace CustomerService.Core.Services
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
        public async Task<UserDto?> CreateUser(SignupDto user, string role)
		{
			if (await IsValidUserName(user.UserName))
				return null;

            var userDto = new UserDto
            {
                UserName = user.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
                MobileNumber = user.MobileNumber ?? string.Empty
            };
            var userModel = mapper.Map<User>(userDto);
			userModel = await _userRepository.CreateUser(userModel, role);
			return mapper.Map<UserDto?>(userModel);
		}

    }
}

