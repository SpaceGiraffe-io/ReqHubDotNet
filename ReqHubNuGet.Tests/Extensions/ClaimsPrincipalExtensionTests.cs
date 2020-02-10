namespace ReqHubNuGet.Tests
{
    using ReqHub;
    using System;
    using System.Security.Claims;
    using Xunit;

    public class ClaimsPrincipalExtensionTests
    {
        [Fact]
        public void TestGetClientId()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ReqHubClaimTypes.ClientId, "5")
            }));

            var claimValue = user.GetClientId();

            Assert.Equal(5, claimValue);
        }

        [Fact]
        public void TestGetClientId_Null()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var claimValue = user.GetClientId();

            Assert.Null(claimValue);
        }
    }
}
