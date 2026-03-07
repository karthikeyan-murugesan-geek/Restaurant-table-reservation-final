using System;
namespace ReservationService.WrapperModels
{
	public class ResponseViewModel<T>
	{
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}

