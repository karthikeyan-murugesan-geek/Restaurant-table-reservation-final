using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerService.DAL.Models
{
	[Table("UserRoleMapping")]
	public class UserRoleMapping
	{
		[Key]
		public long ID { get; set; }
		public long UserID { get; set; }
		[ForeignKey("UserID")]
		public User User { get; set; }
		public long RoleID { get; set; }
		[ForeignKey("RoleID")]
		public Role Role { get; set; }
	}
}

