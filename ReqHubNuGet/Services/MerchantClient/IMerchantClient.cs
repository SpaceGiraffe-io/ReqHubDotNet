using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReqHub
{
    public interface IMerchantClient
    {
        Task<HttpResponseMessage> TrackAsync(HttpRequest request, CancellationToken cancellationToken = default);
    }
}
