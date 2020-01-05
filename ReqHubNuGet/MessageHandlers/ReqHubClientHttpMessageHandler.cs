using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReqHub
{
    public class ReqHubClientHttpMessageHandler : DelegatingHandler
    {
        private string publicKey;
        private string privateKey;

        public ReqHubClientHttpMessageHandler(string publicKey, string privateKey)
        {
            this.publicKey = publicKey;
            this.privateKey = privateKey;
        }

        // Following the algorithm in https://apifriends.com/api-security/api-keys/
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var (token, timestamp, nonce) = HashingUtility.Create(this.publicKey, this.privateKey);

            request.Headers.Add(ReqHubHeaders.ClientTokenHeader, token);
            request.Headers.Add(ReqHubHeaders.ClientTimestampHeader, timestamp);
            request.Headers.Add(ReqHubHeaders.ClientNonceHeader, nonce);
            request.Headers.Add(ReqHubHeaders.ClientPublicKeyHeader, publicKey);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
