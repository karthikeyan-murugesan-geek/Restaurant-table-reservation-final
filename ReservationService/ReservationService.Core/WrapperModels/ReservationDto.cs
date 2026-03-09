
namespace ReservationService.WrapperModels.Core;
public class ReservationDto
{
    public int ReservationID { get; set; }
    public int TableID { get; set; }
    public string? TableName { get; set; }
    public int ReservedByUserId { get; set; }
    public string ReservationDate { get; set; }
    public string TimeSlot { get; set; }
    public int GuestsCount { get; set; }

    public string? Status { get; set; } = "Pending";
}

