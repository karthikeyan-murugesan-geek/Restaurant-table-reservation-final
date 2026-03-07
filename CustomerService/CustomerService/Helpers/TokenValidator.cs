using System;
using CustomerService.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CustomerService.Helpers
{
	public class TokenValidator
	{
        private readonly AppSettings _appSettings;
        public TokenValidator(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public bool ValidateRefreshToken(string refreshToken, out ClaimsPrincipal claimsPrincipal)
		{
            var key = Encoding.UTF8.GetBytes(_appSettings.Jwt.Key);
            var validationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                claimsPrincipal = tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken securityToken);
                return true;
            }
            catch(Exception)
            {
                claimsPrincipal = null;
                return false;
            }
        }
	}
}

