using System;
using CustomerService.Core.Models;

namespace CustomerService.Core.Services.Interfaces
{
	public interface IAuthService
	{
        Task<LoginResultDto?> LoginAsync(LoginDto model);

        LoginResultDto RefreshTokenAsync(TokenInfoDto tokenInfoDto);
    }
}

