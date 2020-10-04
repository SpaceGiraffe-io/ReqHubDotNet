using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ReqHub
{
    public interface IMerchantClient
    {
        Task<HttpResponseMessage> TrackAsync(string path, IDictionary<string, string> headers, CancellationToken cancellationToken = default);

        ClaimsIdentity CreateReqHubIdentity(TrackingResponseModel trackingResponse);
    }
}
