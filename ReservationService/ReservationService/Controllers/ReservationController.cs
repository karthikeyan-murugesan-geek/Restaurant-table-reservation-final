using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationService.DAL.Enum;
using ReservationService.Filters;
using ReservationService.Services;
using ReservationService.Services.Interfaces;
using ReservationService.ViewModel;
using ReservationService.WrapperModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReservationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [CustomAuthorizationFilter("Customer")]
        [HttpGet("GetReservation/{id}")]
        public async Task<IActionResult> GetReservation(long id)
        {
            var reservation = await _reservationService.GetAsync(id);

            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }
        [CustomAuthorizationFilter("Customer")]
        [HttpGet("GetReservationsByUser")]
        public async Task<IActionResult> GetReservationsByUser()
        {
            var userClaims = GetCurrentUser();
            var reservations = await _reservationService.GetByUserAsync(Convert.ToInt64(userClaims.UserID));

            if (reservations == null)
                return NotFound();

            return Ok(reservations);
        }
        [CustomAuthorizationFilter("Manager")]
        [HttpGet("GetAllReservations")]
        public async Task<IActionResult> GetAllReservation(int? skip, int? take)
        {
            var reservations = await _reservationService.GetAllAsync(skip, take);
            return Ok(reservations);
        }
        [CustomAuthorizationFilter("Manager", "Customer")]
        [HttpPost("CreateReservation")]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationViewModel reservationmodel)
        {
            var result = await _reservationService.CreateReservationAsync(reservationmodel);

            return Ok(result);
        }
        [CustomAuthorizationFilter("Manager")]
        [HttpPut("UpdateReservation")]
        public async Task<IActionResult> UpdateReservation([FromBody] ReservationViewModel reservationmodel)
        {
            var result = await _reservationService.UpdateAsync(reservationmodel);

            return Ok(result);
        }
        [CustomAuthorizationFilter("Manager")]
        [HttpPost("DeleteReservation/{id}")]
        public async Task<IActionResult> DeleteReservation(long id)
        {
            var result = await _reservationService.DeleteAsync(id);

            return Ok(result);
        }
        [CustomAuthorizationFilter("Manager")]
        [HttpPost("SearchReservations")]
        public async Task<IActionResult> SearchReservations([FromBody] SearchReservationViewModel searchReservationsModel)
        {
            var result = await _reservationService.GetReservationsByDateSlotAsync(DateOnly.Parse(searchReservationsModel.ReservationDate),
                TimeOnly.Parse(searchReservationsModel.TimeSlot), searchReservationsModel.Skip, searchReservationsModel.Take);
            
            return Ok(result);
        }
        [CustomAuthorizationFilter("Manager")]
        [HttpPatch("{id}/ReservationStatus")]
        public async Task<IActionResult> UpdateReservationStatus(long id, [FromBody] ReservationStatus status)
        {
            var updated = await _reservationService.UpdateStatusAsync(id, status);

            if (updated == null)
                return NotFound();

            return Ok(status.ToString());
        }

        private UserClaimsViewModel GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            var currentUser = new UserClaimsViewModel
            {
                UserID = userIdClaim,
                UserName = userName,
                Roles = roles
            };
            return currentUser;
        }
    }
}

