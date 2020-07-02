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

namespace ReqHubDotNet.Tests.Services
{
    public class MerchantClientTests
    {
        [Fact]
        public void TestTrackAsync()
        {
            var service = this.CreateService();

            var path = "/test";

            var headers = new Dictionary<string, string>
            {
                { ReqHubHeaders.ClientTokenHeader, "test" },
                { ReqHubHeaders.ClientNonceHeader, "test" },
                { ReqHubHeaders.ClientPublicKeyHeader, "test" },
                { ReqHubHeaders.ClientTimestampHeader, "test" },
                { ReqHubHeaders.ClientUrlHeader, "test" }
            };

            var result = service.TrackAsync(path, headers);

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
