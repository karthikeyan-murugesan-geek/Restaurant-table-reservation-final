using Xunit;
using Moq;
using System.Threading.Tasks;
using ReservationService.Services;
using ReservationService.Models;
using AutoMapper;
using System;
using ReservationService.DAL.Models;
using ReservationService.DAL.Repositories.Interfaces;
using ReservationService.ViewModel;
using ReservationService.Services.Interfaces;
using ReservationService.Mappings;

public class ReservationServiceTests
{
    private readonly Mock<IReservationRepository> _reservationRepoMock = new();
    private readonly Mock<ITableRepository> _tableRepoMock = new();
    private readonly Mock<ICustomerApiService> _customerServiceMock = new();
    private readonly IMapper _mapperMock;
    private readonly ReservationService.Services.ReservationService _service;

    public ReservationServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ReservationProfile>();
        });
        _mapperMock = config.CreateMapper();
        var mapper = config.CreateMapper();
        _service = new ReservationService.Services.ReservationService(
            _reservationRepoMock.Object,
            _tableRepoMock.Object,
            _customerServiceMock.Object,
            _mapperMock
        );
    }

    [Fact]
    public async Task CreateReservationAsync_ReturnsSuccess_WhenAllValid()
    {
        // Arrange
        var reservationVM = new ReservationViewModel
        {
            ReservedByUserId = 1,
            TableID = 1,
            GuestsCount = 2,
            ReservationDate = "3/7/2026",
            TimeSlot = "10:00"
        };

        var reservationModel = new Reservation
        {
            ReservedByUserID = 1,
            TableID = 1,
            GuestsCount = 2,
            ReservationDate = new DateOnly(2026,03,07),
            TimeSlot = new TimeOnly(10,00)
        };

        var table = new Table { ID = 1, Capacity = 4 };


        _customerServiceMock.Setup(c => c.CustomerExists(1)).ReturnsAsync(true);
        _tableRepoMock.Setup(t => t.GetAsync(1)).ReturnsAsync(table);
        _reservationRepoMock.Setup(r => r.GetByTableDateSlotAsync(1, reservationModel.ReservationDate, reservationModel.TimeSlot))
            .ReturnsAsync((Reservation?)null);
        _reservationRepoMock.Setup(r => r.AddAsync(It.IsAny<Reservation>()))
            .ReturnsAsync((Reservation r) =>
               {
                   r.ID = 1;
                   return r;
               });

        // Act
        var result = await _service.CreateReservationAsync(reservationVM);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Reservation successful", result.Message);
        Assert.Equal(1, result.Data.ReservationID);
    }

    [Fact]
    public async Task CreateReservationAsync_ReturnsError_WhenCustomerDoesNotExist()
    {
        var reservationVM = new ReservationViewModel { ReservedByUserId = 1, TableID = 1 };

        _customerServiceMock.Setup(c => c.CustomerExists(1)).ReturnsAsync(false);

        var result = await _service.CreateReservationAsync(reservationVM);

        Assert.False(result.Success);
        Assert.Equal("Customer doesnot exists.", result.Message);
    }

    [Fact]
    public async Task CreateReservationAsync_ReturnsError_WhenTableDoesNotExist()
    {
        var reservationVM = new ReservationViewModel { ReservedByUserId = 1, TableID = 1 };

        _customerServiceMock.Setup(c => c.CustomerExists(reservationVM.ReservedByUserId)).ReturnsAsync(true);
        _tableRepoMock.Setup(t => t.GetAsync(1)).ReturnsAsync((Table?)null);

        var result = await _service.CreateReservationAsync(reservationVM);

        Assert.False(result?.Success);
        Assert.Equal("Table does not exists", result.Message);
    }

    [Fact]
    public async Task CreateReservationAsync_ReturnsError_WhenGuestsExceedCapacity()
    {
        var reservationVM = new ReservationViewModel { ReservedByUserId = 1, TableID = 1, GuestsCount = 5 };
        var table = new Table { ID = 1, Capacity = 4 };

        _customerServiceMock.Setup(c => c.CustomerExists(1)).ReturnsAsync(true);
        _tableRepoMock.Setup(t => t.GetAsync(1)).ReturnsAsync(table);

        var result = await _service.CreateReservationAsync(reservationVM);

        Assert.False(result.Success);
        Assert.Equal("Guest count exceeds table capacity", result.Message);
    }

    [Fact]
    public async Task CreateReservationAsync_ReturnsError_WhenTableAlreadyReserved()
    {
        var reservationVM = new ReservationViewModel { ReservedByUserId = 1, TableID = 1, GuestsCount = 2, ReservationDate = "2026/03/07", TimeSlot = "10:00" };
        var table = new Table { ID = 1, Capacity = 4 };
        var existingReservation = new Reservation();

        _customerServiceMock.Setup(c => c.CustomerExists(1)).ReturnsAsync(true);
        _tableRepoMock.Setup(t => t.GetAsync(1)).ReturnsAsync(table);
        _reservationRepoMock.Setup(r => r.GetByTableDateSlotAsync(1, new DateOnly(2026, 03, 07),new TimeOnly(10, 00)))
            .ReturnsAsync(existingReservation);

        var result = await _service.CreateReservationAsync(reservationVM);

        Assert.False(result.Success);
        Assert.Equal("Table already reserved for this time slot", result.Message);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsList()
    {
        // Arrange
        var reservations = GetReservationMockData();

        var mappedReservations = GetReservationViewModelMockData();

        _reservationRepoMock.Setup(r => r.GetAllAsync(null, null))
            .ReturnsAsync(reservations);


        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_WithPagination_ReturnsList()
    {
        // Arrange
        var reservations = GetReservationMockData();

        var mappedReservations = GetReservationViewModelMockData();

        _reservationRepoMock.Setup(r => r.GetAllAsync(0, 2))
            .ReturnsAsync(reservations);


        // Act
        var result = await _service.GetAllAsync(0, 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(mappedReservations.Count, result.Count);
    }

    [Fact]
    public async Task GetReservationsByDateSlotAsync_ReturnsList()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        var time = new TimeOnly(10, 0);

        var reservations = GetReservationMockData();

        var mappedReservations = GetReservationViewModelMockData();

        _reservationRepoMock
            .Setup(r => r.GetReservationsByDateSlotAsync(date, time, null, null))
            .ReturnsAsync(reservations);

        // Act
        var result = await _service.GetReservationsByDateSlotAsync(date, time);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Count, 2);
    }

    [Fact]
    public async Task GetReservationsByDateSlotAsync_WithPagination_ReturnsList()
    {
        // Arrange
        var date = DateOnly.FromDateTime(DateTime.Today);
        var time = new TimeOnly(10, 0);

        var reservations = GetReservationMockData();

        var mappedReservations = GetReservationViewModelMockData();

        _reservationRepoMock
            .Setup(r => r.GetReservationsByDateSlotAsync(date, time, 0, 2))
            .ReturnsAsync(reservations);


        // Act
        var result = await _service.GetReservationsByDateSlotAsync(date, time, 0, 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    private List<Reservation> GetReservationMockData()
    {
        return new List<Reservation>
        {
            new Reservation {
            ReservedByUserID = 1,
            TableID = 1,
            GuestsCount = 2,
            ReservationDate = new DateOnly(2026,03,07),
            TimeSlot = new TimeOnly(10,00)
        },
            new Reservation {
            ReservedByUserID = 1,
            TableID = 2,
            GuestsCount = 3,
            ReservationDate = new DateOnly(2026,03,07),
            TimeSlot = new TimeOnly(10,00)
        }
        };
    }

    private List<ReservationViewModel> GetReservationViewModelMockData()
    {
        return new List<ReservationViewModel>
        {
            new ReservationViewModel { ReservedByUserId = 1,
            TableID = 1,
            GuestsCount = 2,
            ReservationDate = "2026/03/07",
            TimeSlot = "10:00"},
            new ReservationViewModel { ReservedByUserId = 1,
            TableID = 2,
            GuestsCount = 2,
            ReservationDate = "2026/03/07",
            TimeSlot = "10:00" }
        };
    }
}