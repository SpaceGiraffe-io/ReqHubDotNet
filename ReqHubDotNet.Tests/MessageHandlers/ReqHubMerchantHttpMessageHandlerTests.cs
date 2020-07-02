using ReqHub;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ReqHubDotNet.Tests.MessageHandlers
{
    public class ReqHubMerchantHttpMessageHandlerTests
    {
        [Fact]
        public async Task TestSendAsync()
        {
            var handler = this.CreateHandler();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost");
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            var invoker = new HttpMessageInvoker(handler);

            var result = await invoker.SendAsync(request, cancellationToken: default);

            Assert.NotNull(result);
        }

        private ReqHubMerchantHttpMessageHandler CreateHandler()
        {
            var handler = new ReqHubMerchantHttpMessageHandler("test_public", "test_private")
            {
                InnerHandler = new TestHandler()
            };

            return handler;
        }

        // See https://stackoverflow.com/questions/31650718/unit-testing-delegatinghandler
        private class TestHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.OK), cancellationToken);
            }
        }
    }
}
