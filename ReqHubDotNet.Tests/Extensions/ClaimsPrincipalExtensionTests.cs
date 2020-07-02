namespace ReqHubDotNet.Tests
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
                 new Claim(ReqHubClaimTypes.ClientId, "client_5")
            }));

            var claimValue = user.GetClientId();

            Assert.Equal("client_5", claimValue);
        }

        [Fact]
        public void TestGetClientId_Null()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var claimValue = user.GetClientId();

            Assert.Null(claimValue);
        }

        [Fact]
        public void TestGetPlanName()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ReqHubClaimTypes.PlanName, "Test Plan")
            }));

            var claimValue = user.GetPlanName();

            Assert.Equal("Test Plan", claimValue);
        }

        [Fact]
        public void TestGetPlanName_Null()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var claimValue = user.GetPlanName();

            Assert.Null(claimValue);
        }

        [Fact]
        public void TestGetNormalizedPlanName()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ReqHubClaimTypes.NormalizedPlanName, "TEST_PLAN")
            }));

            var claimValue = user.GetNormalizedPlanName();

            Assert.Equal("TEST_PLAN", claimValue);
        }

        [Fact]
        public void TestGetNormalizedPlanName_Null()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var claimValue = user.GetNormalizedPlanName();

            Assert.Null(claimValue);
        }

        [Fact]
        public void TestGetPlanSku()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ReqHubClaimTypes.PlanSku, "Test Plan SKU")
            }));

            var claimValue = user.GetPlanSku();

            Assert.Equal("Test Plan SKU", claimValue);
        }

        [Fact]
        public void TestGetPlanSku_Null()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var claimValue = user.GetPlanSku();

            Assert.Null(claimValue);
        }

        [Fact]
        public void TestGetNormalizedPlanSku()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ReqHubClaimTypes.NormalizedPlanSku, "TEST_PLAN_SKU")
            }));

            var claimValue = user.GetNormalizedPlanSku();

            Assert.Equal("TEST_PLAN_SKU", claimValue);
        }

        [Fact]
        public void TestGetNormalizedPlanSku_Null()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var claimValue = user.GetNormalizedPlanSku();

            Assert.Null(claimValue);
        }
    }
}
