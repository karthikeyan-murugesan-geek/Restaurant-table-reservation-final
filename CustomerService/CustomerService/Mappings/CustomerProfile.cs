using System;
using AutoMapper;
using CustomerService.DAL.Models;
using CustomerService.ViewModel;

namespace CustomerService.Mappings
{
	public class CustomerProfile :Profile
	{
		public CustomerProfile()
		{
			CreateMap<User, UserViewModel>()
				.ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.ID))
				.ForMember(
				dest => dest.Roles,
				opt => opt.MapFrom(src => src.UserRoles.Select(urm => urm.Role.Name).ToList())
			).ReverseMap();

        }
	}
}

