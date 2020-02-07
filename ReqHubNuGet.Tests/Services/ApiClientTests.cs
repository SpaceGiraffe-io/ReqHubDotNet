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
    public class ApiClientTests
    {
        [Fact]
        public void TestGetAsync()
        {
            var service = this.CreateService();

            var result = service.GetAsync<object>("/");

            Assert.NotNull(result);
        }

        [Fact]
        public void TestPostAsync()
        {
            var service = this.CreateService();

            var result = service.PostAsync<object>("/", new StringContent("test"));

            Assert.NotNull(result);
        }

        [Fact]
        public void TestPutAsync()
        {
            var service = this.CreateService();

            var result = service.PutAsync<object>("/", new StringContent("test"));

            Assert.NotNull(result);
        }

        [Fact]
        public void TestPatchAsync()
        {
            var service = this.CreateService();

            var result = service.PatchAsync<object>("/", new StringContent("test"));

            Assert.NotNull(result);
        }

        [Fact]
        public void TestDeleteAsync()
        {
            var service = this.CreateService();

            var result = service.DeleteAsync<object>("/");

            Assert.NotNull(result);
        }

        [Fact]
        public void TestSendAsync()
        {
            var service = this.CreateService();

            var result = service.SendAsync("/", HttpMethod.Get);

            Assert.NotNull(result);
        }

        [Fact]
        public void TestSendAsync_Headers()
        {
            var service = this.CreateService();

            var headers = new Dictionary<string, string>
            {
                { "test", "value" }
            };

            var result = service.SendAsync("/", HttpMethod.Get, headers: headers);

            Assert.NotNull(result);
        }

        [Fact]
        public void TestSendAsync_Message()
        {
            var service = this.CreateService();

            var request = new HttpRequestMessage();

            var result = service.SendAsync(request);

            Assert.NotNull(result);
        }

        private IApiClient CreateService()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();

            var httpClient = new FakeHttpClient();

            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            return new ApiClient(httpClientFactory.Object, "test");
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
