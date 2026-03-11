using System.Globalization;
using FluentValidation;
using ReservationService.Core.WrapperModels;

public class ReservationCreateDtoValidator : AbstractValidator<ReservationCreateDto>
{
    private readonly string[] AllowedTimeSlots =
        { "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00" };

    public ReservationCreateDtoValidator()
    {
        RuleFor(x => x.TableID)
            .GreaterThan(0)
            .WithMessage("TableID is required");

        RuleFor(x => x.ReservationDate)
            .NotEmpty()
            .WithMessage("ReservationDate is required")
            .Must(BeValidDate)
            .WithMessage("ReservationDate must be a valid date in M/d/yyyy format and cannot be in the past");


        RuleFor(x => x.TimeSlot)
            .NotEmpty()
            .WithMessage("TimeSlot is required")
            .Must(slot => AllowedTimeSlots.Contains(slot))
            .WithMessage("TimeSlot must be one of the allowed slots: " + string.Join(", ", AllowedTimeSlots));

        RuleFor(x => x.GuestsCount)
            .GreaterThan(0)
            .WithMessage("GuestsCount must be at least 1")
            .LessThanOrEqualTo(10)
            .WithMessage("GuestsCount cannot exceed 10");
    }

    private bool BeValidDate(string dateStr)
    {
        if (!DateTime.TryParseExact(dateStr, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return false;

        return date >= DateTime.Today;
    }
}