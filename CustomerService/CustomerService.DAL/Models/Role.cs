using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerService.Infrastructure.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public long ID { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }

    }
}

