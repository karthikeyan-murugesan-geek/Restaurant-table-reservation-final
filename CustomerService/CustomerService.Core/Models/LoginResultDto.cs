using System;
namespace CustomerService.Core.Models
{
    public class LoginResultDto
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }
    }
}

