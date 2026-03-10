using System;
using AutoMapper;
using ReservationService.Core.WrapperModels;
using ReservationService.Infrastructure.Enum;
using ReservationService.Infrastructure.Models;
using ReservationService.WrapperModels.Core;

namespace ReservationService.Core.Mappings
{
	public class ReservationProfile :Profile
	{
		public ReservationProfile()
		{
            CreateMap<Reservation, ReservationResponseDto>()
                .ForMember(dest => dest.ReservationID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.TableName, opt => opt.MapFrom(src => src.Table.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ReservationStatus.ToString()))
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate.ToString()))
                .ForMember(dest => dest.TimeSlot, opt => opt.MapFrom(src => src.TimeSlot.ToString()))
                .ReverseMap();

            CreateMap<ReservationCreateDto, Reservation>()
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => DateOnly.Parse(src.ReservationDate)))
                .ForMember(dest => dest.TimeSlot, opt => opt.MapFrom(src => TimeOnly.Parse(src.TimeSlot)))
                .ReverseMap(); 

        }
	}
}

