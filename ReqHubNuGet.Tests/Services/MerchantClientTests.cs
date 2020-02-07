using Microsoft.AspNetCore.Http;
using Moq;
using ReqHub;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ReqHubNuGet.Tests.Services
{
    public class MerchantClientTests
    {
        [Fact]
        public void TestTrackAsync()
        {
            var service = this.CreateService();

            var context = new DefaultHttpContext();

            context.Request.Path = "/test";

            context.Request.Headers.Add(ReqHubHeaders.ClientTokenHeader, "test");
            context.Request.Headers.Add(ReqHubHeaders.ClientTimestampHeader, "test");
            context.Request.Headers.Add(ReqHubHeaders.ClientNonceHeader, "test");
            context.Request.Headers.Add(ReqHubHeaders.ClientPublicKeyHeader, "test");
            context.Request.Headers.Add(ReqHubHeaders.ClientUrlHeader, "test");

            var result = service.TrackAsync(context.Request);

            Assert.NotNull(result);
        }

        private IMerchantClient CreateService()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var httpClient = new FakeHttpClient();

            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            return new MerchantClient(httpClientFactory.Object, "test");
        }

        private class FakeHttpClient : HttpClient
        {
            public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent("{}", Encoding.UTF8, "application/json");
                return Task.FromResult(response);
            }
        }
    }
}
