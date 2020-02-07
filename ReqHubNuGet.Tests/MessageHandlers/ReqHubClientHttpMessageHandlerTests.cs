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

namespace ReqHubNuGet.Tests.MessageHandlers
{
    public class ReqHubClientHttpMessageHandlerTests
    {
        [Fact]
        public async Task TestSendAsync()
        {
            var handler = this.CreateHandler();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost");
            var invoker = new HttpMessageInvoker(handler);

            var result = await invoker.SendAsync(request, cancellationToken: default);

            Assert.NotNull(result);
        }

        private ReqHubClientHttpMessageHandler CreateHandler()
        {
            var handler = new ReqHubClientHttpMessageHandler("test_public", "test_private")
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
