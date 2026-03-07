using System;
using System.Net.Http;
using ReservationService.Models;
using ReservationService.Services.Interfaces;

namespace ReservationService.Services
{
	public class CustomerApiService : ICustomerApiService
    {
		private readonly HttpClient httpClient;
		private readonly ApiSettings apiSettings;
        private readonly IHttpContextAccessor httpContextAccessor;
		public CustomerApiService( HttpClient httpClient, ApiSettings apiSettings, IHttpContextAccessor httpContextAccessor)
		{
			this.httpClient = httpClient;
			this.apiSettings = apiSettings;
            this.httpContextAccessor = httpContextAccessor;
		}

        public async Task<bool> CustomerExists(long userID)
        {
            var url = $"{apiSettings.CustomerService.BaseUrl}/api/Account/IsCustomerAsync/{userID}";
            
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return false;

            var exists = await response.Content.ReadFromJsonAsync<bool>();

            return exists;
        }
    }
}

