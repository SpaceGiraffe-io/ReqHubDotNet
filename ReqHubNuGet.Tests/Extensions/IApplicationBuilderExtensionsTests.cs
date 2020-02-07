using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Moq;
using ReqHub;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ReqHubNuGet.Tests.Extensions
{
    public class IApplicationBuilderExtensionsTests
    {
        [Fact]
        public void TestUseReqHub()
        {
            var applicationBuilder = this.GetApplicationBuilder();

            var result = applicationBuilder.UseReqHub();

            Assert.NotNull(result);
        }

        private IApplicationBuilder GetApplicationBuilder()
        {
            var applicationBuilder = new Mock<IApplicationBuilder>();

            applicationBuilder.Setup(x => x.Use(It.IsAny<Func<RequestDelegate, RequestDelegate>>()))
                .Returns(applicationBuilder.Object);

            return applicationBuilder.Object;
        }
    }
}
