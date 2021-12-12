using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReqHub
{
    public class MerchantClient : IMerchantClient, IDisposable
    {
        private readonly HttpClient httpClient;

        public MerchantClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public MerchantClient(IHttpClientFactory httpClientFactory, string name)
        {
            this.httpClient = httpClientFactory.CreateClient(name);
        }

        public async Task<HttpResponseMessage> VerifyAsync(string path, IDictionary<string, string> headers, CancellationToken cancellationToken = default)
        {
            var json = JsonConvert.SerializeObject(new VerificationRequestModel { RequestUrl = path });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/req", UriKind.Relative),
                Content = content
            };

            // pass client api key headers to the request -- the merchant's headers will be added by the request handler (ReqHubMerchantHttpRequestHandler)
            requestMessage.Headers.Add(ReqHubHeaders.ClientTokenHeader, this.GetHeader(headers, ReqHubHeaders.ClientTokenHeader));
            requestMessage.Headers.Add(ReqHubHeaders.ClientTimestampHeader, this.GetHeader(headers, ReqHubHeaders.ClientTimestampHeader));
            requestMessage.Headers.Add(ReqHubHeaders.ClientNonceHeader, this.GetHeader(headers, ReqHubHeaders.ClientNonceHeader));
            requestMessage.Headers.Add(ReqHubHeaders.ClientPublicKeyHeader, this.GetHeader(headers, ReqHubHeaders.ClientPublicKeyHeader));
            requestMessage.Headers.Add(ReqHubHeaders.ClientUrlHeader, this.GetHeader(headers, ReqHubHeaders.ClientUrlHeader));

            var response = await httpClient.SendAsync(requestMessage, cancellationToken);
            return response;
        }

        public ClaimsIdentity CreateReqHubIdentity(VerificationResponseModel verificationResponse)
        {
            var claims = new List<Claim>();
            this.AddClaim(claims, ReqHubClaimTypes.ClientId, verificationResponse.ClientId);
            this.AddClaim(claims, ReqHubClaimTypes.PlanName, verificationResponse.PlanName);
            this.AddClaim(claims, ReqHubClaimTypes.NormalizedPlanName, verificationResponse.NormalizedPlanName);
            this.AddClaim(claims, ReqHubClaimTypes.PlanSku, verificationResponse.PlanSku);
            this.AddClaim(claims, ReqHubClaimTypes.NormalizedPlanSku, verificationResponse.NormalizedPlanSku);
            this.AddClaim(claims, ReqHubClaimTypes.IsTrial, verificationResponse.IsTrial.ToString());

            var reqHubIdentity = new ClaimsIdentity(claims: claims, authenticationType: "ReqHub");
            return reqHubIdentity;
        }

        public void Dispose()
        {
            this.httpClient.Dispose();
        }

        private string GetHeader(IDictionary<string, string> headers, string key)
        {
            return headers.ContainsKey(key) ? headers[key] : default;
        }

        private void AddClaim(IList<Claim> claims, string claimType, string claimValue)
        {
            if (claimValue != null)
            {
                claims.Add(new Claim(claimType, claimValue));
            }
        }
    }
}
