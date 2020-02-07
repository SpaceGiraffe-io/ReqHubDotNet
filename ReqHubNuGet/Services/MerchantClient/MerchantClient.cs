using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReqHub
{
    public class MerchantClient : IMerchantClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string name;

        public MerchantClient(IHttpClientFactory httpClientFactory, string name)
        {
            this.httpClientFactory = httpClientFactory;
            this.name = name;
        }

        public async Task<HttpResponseMessage> TrackAsync(HttpRequest request, CancellationToken cancellationToken = default)
        {
            var httpClient = this.httpClientFactory.CreateClient(this.name);

            var json = JsonConvert.SerializeObject(new TrackRequestModel { RequestUrl = request.Path });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/tracking", UriKind.Relative),
                Content = content
            };

            // pass client api key headers to the request -- the merchant's headers will be added by the request handler (ReqHubMerchantHttpRequestHandler)
            requestMessage.Headers.Add(ReqHubHeaders.ClientTokenHeader, request.Headers[ReqHubHeaders.ClientTokenHeader].ToString());
            requestMessage.Headers.Add(ReqHubHeaders.ClientTimestampHeader, request.Headers[ReqHubHeaders.ClientTimestampHeader].ToString());
            requestMessage.Headers.Add(ReqHubHeaders.ClientNonceHeader, request.Headers[ReqHubHeaders.ClientNonceHeader].ToString());
            requestMessage.Headers.Add(ReqHubHeaders.ClientPublicKeyHeader, request.Headers[ReqHubHeaders.ClientPublicKeyHeader].ToString());
            requestMessage.Headers.Add(ReqHubHeaders.ClientUrlHeader, request.Headers[ReqHubHeaders.ClientUrlHeader].ToString());

            var response = await httpClient.SendAsync(requestMessage, cancellationToken);

            return response;
        }
    }
}
