using System;
namespace CustomerService.Helpers
{
	public class TokenRequest
	{
		public long UserID { get; set; }
		public string UserName { get; set; }
		public List<string> Roles { get; set; }
		public int ExpireMinutes { get; set; }
	}
}

