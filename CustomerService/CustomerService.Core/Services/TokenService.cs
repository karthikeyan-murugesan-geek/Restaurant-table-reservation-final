using System;
using CustomerService.Core.Helpers;
using CustomerService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CustomerService.Core.Services.Interfaces;
using CustomerService.Core.Models;

namespace CustomerService.Core.Services
{
	public class TokenService : ITokenService
	{
        private readonly AppSettings _appSettings;
        public TokenService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }
        public string GenereteToken(TokenRequestDto tokenRequest)
        {
            var key = Encoding.UTF8.GetBytes(_appSettings.Jwt.Key);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(Convert.ToInt32(tokenRequest.ExpireMinutes));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, tokenRequest.UserID.ToString()),
                new Claim(ClaimTypes.Name, tokenRequest.UserName ?? string.Empty)
            };

            // Add multiple role claims
            foreach (var role in tokenRequest.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
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
            catch (Exception)
            {
                claimsPrincipal = null;
                return false;
            }
        }
    }
}

