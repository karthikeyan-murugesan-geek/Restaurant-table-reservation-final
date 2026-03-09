using System;
using System.Net.Http.Headers;

namespace ReservationService.Middlewares
{
	public class TokenForwardingHandler :DelegatingHandler
	{
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenForwardingHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var token = httpContextAccessor.HttpContext?
                .Request.Headers["Authorization"]
                .ToString();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization =
                    AuthenticationHeaderValue.Parse(token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}

