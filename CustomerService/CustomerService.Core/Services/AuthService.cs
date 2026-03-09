using System;
using System.Security.Claims;
using CustomerService.Core.Helpers;
using CustomerService.Core.Models;
using CustomerService.Core.Services.Interfaces;
using CustomerService.Models;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CustomerService.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService userService;
        private readonly ITokenService tokenService;
        private readonly AppSettings _appSettings;

        public AuthService(IUserService userService, ITokenService tokenService, AppSettings appSettings)
        {
            this.userService = userService;
            this.tokenService = tokenService;
            this._appSettings = appSettings;
        }

        public async Task<LoginResultDto?> LoginAsync(LoginDto model)
        {
            var user = await userService.GetUserByUserName(model.UserName);

            if (user == null)
            {
                return new LoginResultDto
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            bool validPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            if (!validPassword)
            {
                return new LoginResultDto
                {
                    Success = false,
                    Message = "Invalid password"
                };
            }

            var accessToken = tokenService.GenereteToken(new TokenRequestDto
            {
                UserID = user.UserID,
                UserName = user.UserName,
                ExpireMinutes = 60,
                Roles = user.Roles
            });

            var refreshToken = tokenService.GenereteToken(new TokenRequestDto
            {
                UserID = user.UserID,
                UserName = user.UserName,
                ExpireMinutes = 1440,
                Roles = user.Roles
            });

            return new LoginResultDto
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }


        public LoginResultDto RefreshTokenAsync(TokenInfoDto tokenInfoDto)
        {
            bool valid = tokenService.ValidateRefreshToken(tokenInfoDto.RefreshToken, out ClaimsPrincipal claimsPrincipal);

            if (!valid)
            {
                return new LoginResultDto
                {
                    Success = false,
                    Message = "Token Invalid"
                };
            }

            var userID = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
            var roles = claimsPrincipal.FindAll(ClaimTypes.Role)
                     .Select(r => r.Value)
                     .ToList();

            var accessToken = tokenService.GenereteToken(new TokenRequestDto
            {
                UserID = Convert.ToInt64(userID),
                UserName = userName ?? string.Empty,
                ExpireMinutes = 60,
                Roles = roles
            });


            var refreshToken = tokenService.GenereteToken(new TokenRequestDto
            {
                UserID = Convert.ToInt64(userID),
                UserName = userName ?? string.Empty,
                ExpireMinutes = 1440,
                Roles = roles
            });

            return new LoginResultDto
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

        }
    }
}

