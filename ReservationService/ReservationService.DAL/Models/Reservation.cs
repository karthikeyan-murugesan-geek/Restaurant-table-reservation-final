using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReservationService.DAL.Enum;

namespace ReservationService.DAL.Models
{
	public class Reservation
	{
        [Key]
        public long ID { get; set; }

        [Required]
        public long TableID { get; set; }

        [ForeignKey("TableID")]
        public Table Table { get; set; }

        [Required]
        public long ReservedByUserID { get; set; }

        [Required]
        public DateOnly ReservationDate { get; set; }

        [Required]
        public TimeOnly TimeSlot { get; set; }

        [Required]
        public int GuestsCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ReservationStatus ReservationStatus { get; set; } = ReservationStatus.Pending;
    }
}

