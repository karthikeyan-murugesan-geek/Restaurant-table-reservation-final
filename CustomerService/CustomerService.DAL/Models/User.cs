using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerService.DAL.Models
{
    [Table("User")]
	public class User
	{
        [Key]
        public long ID { get; set; }
        [Required]
        [Column(TypeName ="VARCHAR(200)")]
        public string UserName { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(500)")]
        public string PasswordHash { get; set; }
        [Column(TypeName ="VARCHAR(20)")]
        public string MobileNumber { get; set; }
        public ICollection<UserRoleMapping> UserRoles { get; set; } = new List<UserRoleMapping>();
    }
}
