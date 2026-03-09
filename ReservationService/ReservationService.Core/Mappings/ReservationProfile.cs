using System;
using AutoMapper;
using ReservationService.Infrastructure.Enum;
using ReservationService.Infrastructure.Models;
using ReservationService.WrapperModels.Core;

namespace ReservationService.Core.Mappings
{
	public class ReservationProfile :Profile
	{
		public ReservationProfile()
		{
            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.ReservationID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.TableName, opt => opt.MapFrom(src => src.Table.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ReservationStatus.ToString()))
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate.ToString()))
                .ForMember(dest => dest.TimeSlot, opt => opt.MapFrom(src => src.TimeSlot.ToString()));

            CreateMap<ReservationDto, Reservation>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ReservationID))
                .ForMember(dest => dest.ReservationStatus, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.Status) ? ReservationStatus.Pending : Enum.Parse<ReservationStatus>(src.Status)))
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => DateOnly.Parse(src.ReservationDate)))
                .ForMember(dest => dest.TimeSlot, opt => opt.MapFrom(src => TimeOnly.Parse(src.TimeSlot))); 

        }
	}
}

