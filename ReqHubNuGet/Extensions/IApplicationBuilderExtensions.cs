using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReqHub
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseReqHub(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ReqHubMerchantMiddleware>();
        }
    }
}
