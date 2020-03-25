using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReqHub
{
    public class ReqHubMerchantMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IMerchantClient merchantClient;

        public ReqHubMerchantMiddleware(RequestDelegate next, IMerchantClient merchantClient)
        {
            this.next = next;
            this.merchantClient = merchantClient;
        }

        public async Task Invoke(HttpContext context)
        {
            var response = await this.merchantClient.TrackAsync(context.Request);

            if (response.IsSuccessStatusCode)
            {
                var trackingResponseJson = await response.Content.ReadAsStringAsync();
                var trackingResponse = JsonConvert.DeserializeObject<TrackingResponseModel>(trackingResponseJson);

                var claims = new List<Claim>
                {
                    new Claim(ReqHubClaimTypes.ClientId, trackingResponse.ClientId)
                };

                var reqHubIdentity = new ClaimsIdentity(claims: claims, authenticationType: "ReqHub");
                context.User.AddIdentity(reqHubIdentity);

                await this.next(context);
            }
            else
            {
                var message = await response.Content?.ReadAsStringAsync();
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync($"403 Forbidden. {message}");
            }
        }
    }
}
