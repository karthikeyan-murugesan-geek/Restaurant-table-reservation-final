using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationService.DAL.Models
{
    [Table("Table")]
	public class Table
	{
        [Key]
        public long ID { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(200)")]
        public string Name { get; set; } 

        [Required]
        public int Capacity { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}

