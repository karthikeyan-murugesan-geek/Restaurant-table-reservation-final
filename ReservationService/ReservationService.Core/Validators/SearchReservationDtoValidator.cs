using System;
using System.Globalization;
using FluentValidation;
using ReservationService.Core.WrapperModels;

namespace ReservationService.Core.Validators
{
	public class SearchReservationDtoValidator : AbstractValidator<SearchReservationDto>
    {
        private readonly string[] AllowedTimeSlots =
        { "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00" };

        public SearchReservationDtoValidator()
		{
            RuleFor(x => x.ReservationDate)
            .NotEmpty()
            .WithMessage("ReservationDate is required")
            .Must(BeValidDate)
            .WithMessage("ReservationDate must be a valid date in M/d/yyyy format");


            RuleFor(x => x.TimeSlot)
                .NotEmpty()
                .WithMessage("TimeSlot is required")
                .Must(slot => AllowedTimeSlots.Contains(slot))
                .WithMessage("TimeSlot must be one of the allowed slots: " + string.Join(", ", AllowedTimeSlots));
        }

        private bool BeValidDate(string dateStr)
        {
            if (!DateTime.TryParseExact(dateStr, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                return false;

            return true;
        }
    }
}

