using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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
                await this.next(context);
            }
            else
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("403 Forbidden.");
            }
        }
    }
}
