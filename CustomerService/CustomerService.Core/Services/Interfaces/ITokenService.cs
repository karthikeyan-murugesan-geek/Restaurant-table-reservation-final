using System;
using System.Security.Claims;
using CustomerService.Core.Helpers;
using CustomerService.Core.Models;

namespace CustomerService.Core.Services.Interfaces
{
	public interface ITokenService
	{
        string GenereteToken(TokenRequestDto tokenRequest);
        bool ValidateRefreshToken(string refreshToken, out ClaimsPrincipal claimsPrincipal);

    }
}

