using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CustomerService.Infrastructure.Models;
using CustomerService.Models;
using CustomerService.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace CustomerService.Helpers
{
	public class TokenGeneretor
	{
		private readonly AppSettings _appSettings;
		public TokenGeneretor(AppSettings appSettings)
		{
			_appSettings = appSettings;
		}
		public string GenereteToken(TokenRequest tokenRequest)
		{
			var key = Encoding.UTF8.GetBytes(_appSettings.Jwt.Key);
			var tokenHandler = new JwtSecurityTokenHandler();

			var now = DateTime.UtcNow;
			var expires = now.AddMinutes(Convert.ToInt32(tokenRequest.ExpireMinutes));
            var claims = new List<Claim>
            {
				new Claim(ClaimTypes.NameIdentifier, tokenRequest.UserID.ToString()),
				new Claim(ClaimTypes.Name, tokenRequest.UserName)
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
	}
}

