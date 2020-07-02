using Microsoft.Extensions.DependencyInjection;
using Moq;
using ReqHub;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Sdk;

namespace ReqHubDotNet.Tests.Extensions
{
    public class IServiceCollectionExtensionsTests
    {
        [Fact]
        public void TestAddApiClient()
        {
            var serviceCollection = this.GetServiceCollection();

            try
            {
                serviceCollection.AddApiClient("https://localhost", "public_test", "private_test");
            }
            catch (Exception)
            {
                throw new XunitException("Expected not to throw.");
            }
        }

        [Fact]
        public void TestAddReqHub()
        {
            var serviceCollection = this.GetServiceCollection();

            try
            {
                serviceCollection.AddReqHub("public_test", "private_test");
            }
            catch (Exception)
            {
                throw new XunitException("Expected not to throw.");
            }
        }

        private IServiceCollection GetServiceCollection()
        {
            var serviceCollectionMock = new Mock<IServiceCollection>();
            serviceCollectionMock.Setup(x => x.GetEnumerator()).Returns(new List<ServiceDescriptor>().GetEnumerator());

            var serviceCollection = serviceCollectionMock.Object;

            return serviceCollection;
        }
    }
}
