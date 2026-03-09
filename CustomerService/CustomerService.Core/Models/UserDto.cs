namespace CustomerService.Core.Models;
public class UserDto
{
    public long UserID { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string MobileNumber { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}

