using System.Security.Claims;
using CustomerService.Core.Models;
using CustomerService.Helpers;
using CustomerService.Models;
using CustomerService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomerService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private TokenGeneretor tokenGeneretor;
        private TokenValidator tokenValidator;
        private readonly AppSettings appSettings;
        public AccountController(IUserService userService, AppSettings appSettings)
        {
            this.userService = userService;
            this.appSettings = appSettings;
            tokenGeneretor = new TokenGeneretor(appSettings);
            tokenValidator = new TokenValidator(appSettings);
        }
        
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignupDto signUpModel)
        {
            if (await userService.IsValidUserName(signUpModel.UserName))
                return BadRequest("Username already exists");

            var userViewModel = new UserDto
            {
                UserName = signUpModel.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(signUpModel.Password),
                MobileNumber = signUpModel.MobileNumebr ?? string.Empty
            };

            var user = await userService.CreateUser(userViewModel, signUpModel.Role);

            return Ok($"User created as {signUpModel.Role} {user.UserName}");
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // Get user from DB
            var user = await userService.GetUserByUserName(model.UserName);

            if (user == null)
                return Unauthorized("User not found");

            // Verify password hash
            bool validPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

            if (!validPassword)
                return Unauthorized("Invalid password");

            // Generate JWT Access
            var accessToken = tokenGeneretor.GenereteToken(new TokenRequest
            {
                UserID = user.UserID,
                UserName = user.UserName,
                ExpireMinutes = 60,
                Roles = user.Roles
            });

            // Generate JWT Refresh
            var refreshToken = tokenGeneretor.GenereteToken(new TokenRequest
            {
                UserID = user.UserID,
                UserName = user.UserName,
                ExpireMinutes = 1440,
                Roles = user.Roles
            });

            return Ok(new { accessToken, refreshToken });
        }

        [AllowAnonymous]
        [HttpPost("refreshAccessToken")]
        public IActionResult refreshAccessToken([FromBody] TokenInfoDto tokenInfoModel)
        {
            // Get user from DB
            bool valid = tokenValidator.ValidateRefreshToken(tokenInfoModel.RefreshToken, out ClaimsPrincipal claimsPrincipal);

            if (!valid)
                return Unauthorized("Token Invalid");

            var userID = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
            var roles = claimsPrincipal.FindAll(ClaimTypes.Role)
                     .Select(r => r.Value)
                     .ToList();
            // Generate JWT
            var accessToken = tokenGeneretor.GenereteToken(new TokenRequest
            {
                UserID = Convert.ToInt64(userID),
                UserName = userName ?? string.Empty ,
                ExpireMinutes = 60,
                Roles = roles
            });

            // Generate JWT
            var refreshToken = tokenGeneretor.GenereteToken(new TokenRequest
            {
                UserID = Convert.ToInt64(userID),
                UserName = userName ?? string.Empty,
                ExpireMinutes = 1440,
                Roles = roles
            });

            return Ok(new { accessToken, refreshToken });
        }

        [HttpGet("IsCustomerAsync/{userID}")]
        public async Task<IActionResult> IsCustomerAsync(long userID)
        {
            return Ok(await userService.IsCustomerAsync(userID));
        }
    }
}

