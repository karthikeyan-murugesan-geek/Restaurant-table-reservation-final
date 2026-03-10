using System;
using AutoMapper;
using ReservationService.Infrastructure.Enum;
using ReservationService.Infrastructure.Models;
using ReservationService.Infrastructure.Repositories.Interfaces;
using ReservationService.Core.Services.Interfaces;
using ReservationService.WrapperModels.Core;
using ReservationService.Core.WrapperModels;

namespace ReservationService.Services
{
	public class ReservationService :IReservationService
	{
		private readonly IReservationRepository _reservationRepository;
        private readonly ITableRepository _tableRepository;
        private readonly ICustomerApiService _customerApiService;
        private readonly IMapper mapper;
		public ReservationService(IReservationRepository reservationRepositpory, ITableRepository tableRepository,
            ICustomerApiService customerApiService ,IMapper mapper)
		{
			this._reservationRepository = reservationRepositpory;
            _tableRepository = tableRepository;
            this._customerApiService = customerApiService;
			this.mapper = mapper;
		}
        public async Task<ResponseDto<ReservationCreateDto>> CreateReservationAsync(ReservationCreateDto reservation)
        {
            var reservationModel = mapper.Map<Reservation>(reservation);
            var isValidCustomer = await _customerApiService.CustomerExists(reservationModel.ReservedByUserID);
            if(!isValidCustomer)
            {
                return new ResponseDto<ReservationCreateDto>
                {
                    Success = false,
                    Message = "Customer doesnot exists."
                };
            }
            var table = await _tableRepository.GetAsync(reservationModel.TableID);
            if (table == null)
            {
                return new ResponseDto<ReservationCreateDto>
                {
                    Success = false,
                    Message = "Table does not exists"
                };
            }
            if(table.Capacity < reservationModel.GuestsCount)
            {
                return new ResponseDto<ReservationCreateDto>
                {
                    Success = false,
                    Message = "Guest count exceeds table capacity"
                };
            }
            var exitingReservation = await _reservationRepository.GetByTableDateSlotAsync(reservationModel.TableID, reservationModel.ReservationDate, reservationModel.TimeSlot);
            if(exitingReservation != null)
            {
                return new ResponseDto<ReservationCreateDto>
                {
                    Success = false,
                    Message = "Table already reserved for this time slot"
                };
            }
            reservationModel = await _reservationRepository.AddAsync(reservationModel);
            return new ResponseDto<ReservationCreateDto>
            {
                Success = true,
                Message = "Reservation successful",
                Data = mapper.Map<ReservationCreateDto>(reservationModel)

            }; 
		}

        public async Task<ReservationCreateDto?> UpdateAsync(long id, ReservationCreateDto reservation)
        {
            var reservationModel = mapper.Map<Reservation>(reservation);
            reservationModel.ID = id;
            reservationModel = await _reservationRepository.UpdateAsync(reservationModel);
            return mapper.Map<ReservationCreateDto?>(reservationModel);
        }

        public async Task<bool> DeleteAsync(long reservationID)
        {
            return await _reservationRepository.DeleteAsync(reservationID);
        }

        public async Task<ReservationResponseDto?> GetAsync(long reservationID)
        {
            var reservationModel = await _reservationRepository.GetAsync(reservationID);
            return mapper.Map<ReservationResponseDto?>(reservationModel);
        }

        public async Task<List<ReservationResponseDto>?> GetByUserAsync(long userID)
        {
            var reservationModelList = await _reservationRepository.GetByUserAsync(userID);
            return mapper.Map<List<ReservationResponseDto>?>(reservationModelList);
        }
        public async Task<List<ReservationResponseDto>?> GetAllAsync(int? skip = null, int? take = null)
        {
            var reservationList = await _reservationRepository.GetAllAsync(skip, take);
            return mapper.Map<List<ReservationResponseDto>?>(reservationList);
        }

        public async Task<List<ReservationResponseDto>?> GetReservationsByDateSlotAsync(DateOnly reservationDate, TimeOnly timeslot, int? skip = null, int? take = null)
        {
            var reservationList = await _reservationRepository.GetReservationsByDateSlotAsync(reservationDate, timeslot, skip, take);
            return mapper.Map<List<ReservationResponseDto>?>(reservationList);
        }

        public async Task<ReservationResponseDto?> UpdateStatusAsync(long id, ReservationStatus reservationStatus)
        {
            var reservationModel = await _reservationRepository.UpdateStatusAsync(id, reservationStatus);
            return mapper.Map<ReservationResponseDto?>(reservationModel);
        }
    }
}

