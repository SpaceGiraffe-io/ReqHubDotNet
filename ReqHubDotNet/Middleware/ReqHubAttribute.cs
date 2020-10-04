using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ReqHub
{
    [ExcludeFromCodeCoverage] // not sure how to test -- dependencies are fixed and IsAuthorized() isn't public
    public class ReqHubAttribute : AuthorizeAttribute
    {
        private static HttpClient httpClient;

        private readonly IMerchantClient merchantClient;

        public ReqHubAttribute(string publicKey, string privateKey, string baseAddress = "https://api.reqhub.io")
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.Create(new ReqHubMerchantHttpMessageHandler(publicKey, privateKey));
                httpClient.BaseAddress = new Uri(baseAddress);
            }
            this.merchantClient = new MerchantClient(httpClient);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var path = actionContext.Request.RequestUri.LocalPath;
            var headers = actionContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value.FirstOrDefault());
            var response = Task.Run(() => this.merchantClient.TrackAsync(path, headers)).Result;

            if (response.IsSuccessStatusCode)
            {
                var trackingResponseJson = response.Content.ReadAsStringAsync().Result;
                var trackingResponse = JsonConvert.DeserializeObject<TrackingResponseModel>(trackingResponseJson);

                var identity = this.merchantClient.CreateReqHubIdentity(trackingResponse);
                var claimsPrincipal = new ClaimsPrincipal(identity);
                actionContext.RequestContext.Principal = claimsPrincipal;
            }
            else
            {
                var message = response.Content.ReadAsStringAsync().Result;
                actionContext.Response = new HttpResponseMessage();
                actionContext.Response.StatusCode = response.StatusCode;
                actionContext.Response.Content = new StringContent($"{(int)response.StatusCode} {response.ReasonPhrase}. {message}");
            }

            return true; // if actionContext.Response is set, the controller doesn't get invoked
        }
    }
}
