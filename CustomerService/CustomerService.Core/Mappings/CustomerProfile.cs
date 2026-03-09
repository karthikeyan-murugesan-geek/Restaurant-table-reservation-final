using System;
using AutoMapper;
using CustomerService.Infrastructure.Models;
using CustomerService.Core.Models;

namespace CustomerService.Mappings
{
	public class CustomerProfile :Profile
	{
		public CustomerProfile()
		{
			CreateMap<User, UserDto>()
				.ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.ID))
				.ForMember(
				dest => dest.Roles,
				opt => opt.MapFrom(src => src.UserRoles.Select(urm => urm.Role.Name).ToList())
			).ReverseMap();

        }
	}
}

