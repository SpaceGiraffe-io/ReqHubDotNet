using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReqHub
{
    public class ReqHubMerchantHttpMessageHandler : DelegatingHandler
    {
        private string publicKey;
        private string privateKey;

        public ReqHubMerchantHttpMessageHandler(string publicKey, string privateKey)
        {
            this.publicKey = publicKey;
            this.privateKey = privateKey;
        }

        // Following the algorithm in https://apifriends.com/api-security/api-keys/
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Include the incoming client request url in the hash
            var requestContent = await request.Content.ReadAsAsync<VerificationRequestModel>();
            var requestUrl = requestContent.RequestUrl;

            var (token, timestamp, nonce) = HashingUtility.Create(this.publicKey, this.privateKey, requestUrl: requestUrl);

            request.Headers.Add(ReqHubHeaders.MerchantTokenHeader, token);
            request.Headers.Add(ReqHubHeaders.MerchantTimestampHeader, timestamp);
            request.Headers.Add(ReqHubHeaders.MerchantNonceHeader, nonce);
            request.Headers.Add(ReqHubHeaders.MerchantPublicKeyHeader, this.publicKey);
            request.Headers.Add(ReqHubHeaders.MerchantUrlHeader, requestUrl);

            // Client headers added in ReqHubMerchantMiddleware > MerchantClient.VerifyAsync()
            // The full trace is Middleware -> VerifyAsync -> SendAsync -> this
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
