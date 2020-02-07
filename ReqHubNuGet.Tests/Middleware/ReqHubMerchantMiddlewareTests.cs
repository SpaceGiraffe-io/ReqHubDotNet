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

namespace ReqHubNuGet.Tests.Middleware
{
    public class ReqHubMerchantMiddlewareTests
    {
        [Fact]
        public async Task TestInvoke()
        {
            var middleware = this.CreateMiddleware();

            var httpContext = new DefaultHttpContext();

            await middleware.Invoke(httpContext);

            Assert.NotEqual(403, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task TestInvoke_Forbidden()
        {
            var middleware = this.CreateMiddleware(isSuccessStatusCode: false);

            var httpContext = new DefaultHttpContext();

            await middleware.Invoke(httpContext);

            Assert.Equal(403, httpContext.Response.StatusCode);
        }

        private ReqHubMerchantMiddleware CreateMiddleware(bool isSuccessStatusCode = true)
        {
            var merchantClientMock = new Mock<IMerchantClient>();

            var statusCode = isSuccessStatusCode ? HttpStatusCode.OK : HttpStatusCode.UnavailableForLegalReasons;

            merchantClientMock.Setup(x => x.TrackAsync(It.IsAny<HttpRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(statusCode));

            RequestDelegate requestDelegate = (context) => Task.CompletedTask;

            var middleware = new ReqHubMerchantMiddleware(requestDelegate, merchantClientMock.Object);

            return middleware;
        }
    }
}
