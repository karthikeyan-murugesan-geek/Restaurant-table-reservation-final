using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ReservationService.Controllers;
using ReservationService.Services.Interfaces;
using ReservationService.ViewModel;
using ReservationService.WrapperModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class ReservationControllerTests
{
    private readonly Mock<IReservationService> _reservationServiceMock;
    private readonly ReservationController _controller;

    public ReservationControllerTests()
    {
        _reservationServiceMock = new Mock<IReservationService>();
        _controller = new ReservationController(_reservationServiceMock.Object);
    }
    private void SetUser(string role, string userId = "1")
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [Fact]
    public async Task CreateReservation_ReturnsOk_WithCreatedReservation()
    {
        
        var reservation = new ReservationViewModel { ReservationID = 1, TableID = 1, GuestsCount = 4 };
        var model = new ResponseViewModel<ReservationViewModel>
        {
            Data = reservation
        };
        _reservationServiceMock.Setup(s => s.CreateReservationAsync(reservation))
            .ReturnsAsync(model);

        
        var result = await _controller.CreateReservation(reservation);

        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(model, okResult.Value);
    }

    [Fact]
    public async Task GetReservation_ReturnsOk_WhenReservationExists()
    {
        var reservation = new ReservationViewModel { ReservationID = 1 };
        
        _reservationServiceMock.Setup(s => s.GetAsync(1))
            .ReturnsAsync(reservation);

        var result = await _controller.GetReservation(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(reservation, okResult.Value);
    }

    [Fact]
    public async Task GetReservationsByUser_ReturnsOk_WhenReservationsExist()
    {
        var reservations = new List<ReservationViewModel>
        {
            new ReservationViewModel { ReservationID = 1 },
            new ReservationViewModel { ReservationID = 2 }
        };

        _reservationServiceMock.Setup(s => s.GetByUserAsync(1))
            .ReturnsAsync(reservations);
        SetUser("Customer");
        var result = await _controller.GetReservationsByUser();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(reservations, okResult.Value);
    }

    [Fact]
    public async Task GetAllReservation_ReturnsOk_WithPaginatedList()
    {
        var paginatedReservations = new List<ReservationViewModel>
        {
            new ReservationViewModel { ReservationID = 1 },
            new ReservationViewModel { ReservationID = 2 }
        };

        _reservationServiceMock.Setup(s => s.GetAllAsync(0, 2))
            .ReturnsAsync(paginatedReservations);

        var result = await _controller.GetAllReservation(0, 2);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(paginatedReservations, okResult.Value);
    }

    [Fact]
    public async Task SearchReservations_ReturnsOk_WithFilteredList()
    {
        var searchModel = new ReservationService.WrapperModels.SearchReservationViewModel
        {
            ReservationDate = "2026-03-07",
            TimeSlot = "10:00",
            Skip = 0,
            Take = 5
        };

        var filteredReservations = new List<ReservationViewModel>
        {
            new ReservationViewModel { ReservationID = 1 }
        };

        _reservationServiceMock.Setup(s => s.GetReservationsByDateSlotAsync(
                DateOnly.Parse(searchModel.ReservationDate),
                TimeOnly.Parse(searchModel.TimeSlot),
                searchModel.Skip,
                searchModel.Take))
            .ReturnsAsync(filteredReservations);

        var result = await _controller.SearchReservations(searchModel);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(filteredReservations, okResult.Value);
    }
}