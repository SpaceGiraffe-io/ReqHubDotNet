using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    public class ReqHubAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private HttpClient httpClient;

        private readonly IMerchantClient merchantClient;

        public ReqHubAttribute(string publicKey, string privateKey, string baseAddress = "https://api.reqhub.io")
        {
            if (this.httpClient == null)
            {
                this.httpClient = HttpClientFactory.Create(new ReqHubMerchantHttpMessageHandler(publicKey, privateKey));
                this.httpClient.BaseAddress = new Uri(baseAddress);
            }
            this.merchantClient = new MerchantClient(this.httpClient);
        }

        // .Net Core
        public async Task OnAuthorizationAsync(AuthorizationFilterContext filterContext)
        {
            var context = filterContext.HttpContext;
            var path = context.Request.Path;
            var headers = context.Request.Headers.ToDictionary(x => x.Key, x => x.Value.FirstOrDefault());
            var response = await this.merchantClient.VerifyAsync(path, headers);

            if (response.IsSuccessStatusCode)
            {
                var verificationResponseJson = response.Content.ReadAsStringAsync().Result;
                var verificationResponse = JsonConvert.DeserializeObject<VerificationResponseModel>(verificationResponseJson);

                var identity = this.merchantClient.CreateReqHubIdentity(verificationResponse);
                context.User.AddIdentity(identity);
            }
            else
            {
                var message = response.Content.ReadAsStringAsync().Result;
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync($"{(int)response.StatusCode} {response.ReasonPhrase}. {message}");
                filterContext.Result = new ForbidResult();
            }
        }

        // .Net Framework
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var path = actionContext.Request.RequestUri.LocalPath;
            var headers = actionContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value.FirstOrDefault());
            var response = Task.Run(() => this.merchantClient.VerifyAsync(path, headers)).Result;

            if (response.IsSuccessStatusCode)
            {
                var verificationResponseJson = response.Content.ReadAsStringAsync().Result;
                var verificationResponse = JsonConvert.DeserializeObject<VerificationResponseModel>(verificationResponseJson);

                var identity = this.merchantClient.CreateReqHubIdentity(verificationResponse);
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
