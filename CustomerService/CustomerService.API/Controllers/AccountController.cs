using System.Security.Claims;
using CustomerService.Core.Models;
using CustomerService.Core.Services.Interfaces;
using CustomerService.Models;
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
        private readonly IAuthService authService;
        public AccountController(IUserService userService, AppSettings appSettings,
            ITokenService tokenService, IAuthService authService)
        {
            this.userService = userService;
            this.authService = authService;
        }
        
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignupDto signUpModel)
        {

            var user = await userService.CreateUser(signUpModel);
            if (user == null)
                return BadRequest("Username already exists");
            return Ok($"User created as {signUpModel.Role} {user.UserName}");
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginModel)
        {
            var loginResult = await authService.LoginAsync(loginModel);

            if (!loginResult.Success)
                return Unauthorized("User not found");

            return Ok(new { loginResult.AccessToken, loginResult.RefreshToken });
        }

        [AllowAnonymous]
        [HttpPost("refreshAccessToken")]
        public IActionResult refreshAccessToken([FromBody] TokenInfoDto tokenInfoModel)
        {
            var tokenResult = authService.RefreshTokenAsync(tokenInfoModel);

            if (!tokenResult.Success)
                return Unauthorized("Invalid token");

            return Ok(new { tokenResult.AccessToken, tokenResult.RefreshToken });
        }

        [HttpGet("GetCustomer/{userID}")]
        public async Task<IActionResult> GetCustomerAsync(long userID)
        {
            return Ok(await userService.IsCustomerAsync(userID));
        }
    }
}

