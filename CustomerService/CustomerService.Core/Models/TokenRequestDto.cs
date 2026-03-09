using System;
namespace CustomerService.Core.Helpers
{
	public class TokenRequestDto
	{
		public long UserID { get; set; }
		public string? UserName { get; set; }
		public List<string> Roles { get; set; } = new();
		public int ExpireMinutes { get; set; }
	}
}

