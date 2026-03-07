using System;
using AutoMapper;
using ReservationService.DAL.Enum;
using ReservationService.DAL.Models;
using ReservationService.DAL.Repositories.Interfaces;
using ReservationService.Services.Interfaces;
using ReservationService.ViewModel;
using ReservationService.WrapperModels;

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
        public async Task<ResponseViewModel<ReservationViewModel>> CreateReservationAsync(ReservationViewModel reservation)
        {
            var reservationModel = mapper.Map<Reservation>(reservation);
            var isValidCustomer = await _customerApiService.CustomerExists(reservationModel.ReservedByUserID);
            if(!isValidCustomer)
            {
                return new ResponseViewModel<ReservationViewModel>
                {
                    Success = false,
                    Message = "Customer doesnot exists."
                };
            }
            var table = await _tableRepository.GetAsync(reservationModel.TableID);
            if (table == null)
            {
                return new ResponseViewModel<ReservationViewModel>
                {
                    Success = false,
                    Message = "Table does not exists"
                };
            }
            if(table.Capacity < reservationModel.GuestsCount)
            {
                return new ResponseViewModel<ReservationViewModel>
                {
                    Success = false,
                    Message = "Guest count exceeds table capacity"
                };
            }
            var exitingReservation = await _reservationRepository.GetByTableDateSlotAsync(reservationModel.TableID, reservationModel.ReservationDate, reservationModel.TimeSlot);
            if(exitingReservation != null)
            {
                return new ResponseViewModel<ReservationViewModel>
                {
                    Success = false,
                    Message = "Table already reserved for this time slot"
                };
            }
            reservationModel = await _reservationRepository.AddAsync(reservationModel);
            return new ResponseViewModel<ReservationViewModel>
            {
                Success = true,
                Message = "Reservation successful",
                Data = mapper.Map<ReservationViewModel>(reservationModel)

            }; 
		}

        public async Task<ReservationViewModel?> UpdateAsync(ReservationViewModel reservation)
        {
            var reservationModel = mapper.Map<Reservation>(reservation);
            reservationModel = await _reservationRepository.UpdateAsync(reservationModel);
            return mapper.Map<ReservationViewModel?>(reservationModel);
        }

        public async Task<bool> DeleteAsync(long reservationID)
        {
            return await _reservationRepository.DeleteAsync(reservationID);
        }

        public async Task<ReservationViewModel?> GetAsync(long reservationID)
        {
            var reservationModel = await _reservationRepository.GetAsync(reservationID);
            return mapper.Map<ReservationViewModel?>(reservationModel);
        }

        public async Task<List<ReservationViewModel>?> GetByUserAsync(long userID)
        {
            var reservationModelList = await _reservationRepository.GetByUserAsync(userID);
            return mapper.Map<List<ReservationViewModel>?>(reservationModelList);
        }
        public async Task<List<ReservationViewModel>?> GetAllAsync(int? skip = null, int? take = null)
        {
            var reservationList = await _reservationRepository.GetAllAsync(skip, take);
            return mapper.Map<List<ReservationViewModel>?>(reservationList);
        }

        public async Task<List<ReservationViewModel>?> GetReservationsByDateSlotAsync(DateOnly reservationDate, TimeOnly timeslot, int? skip = null, int? take = null)
        {
            var reservationList = await _reservationRepository.GetReservationsByDateSlotAsync(reservationDate, timeslot, skip, take);
            return mapper.Map<List<ReservationViewModel>?>(reservationList);
        }

        public async Task<ReservationViewModel?> UpdateStatusAsync(long id, ReservationStatus reservationStatus)
        {
            var reservationModel = await _reservationRepository.UpdateStatusAsync(id, reservationStatus);
            return mapper.Map<ReservationViewModel?>(reservationModel);
        }
    }
}

