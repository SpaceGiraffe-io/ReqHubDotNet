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
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var (token, timestamp, nonce) = HashingUtility.Create(this.publicKey, this.privateKey);

            request.Headers.Add(ReqHubHeaders.MerchantTokenHeader, token);
            request.Headers.Add(ReqHubHeaders.MerchantTimestampHeader, timestamp);
            request.Headers.Add(ReqHubHeaders.MerchantNonceHeader, nonce);
            request.Headers.Add(ReqHubHeaders.MerchantPublicKeyHeader, publicKey);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
