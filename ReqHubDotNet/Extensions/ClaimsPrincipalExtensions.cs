using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ReqHub
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetClientId(this ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(x => string.Equals(x.Type, ReqHubClaimTypes.ClientId, StringComparison.Ordinal));
            return claim?.Value;
        }

        public static string GetPlanName(this ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(x => string.Equals(x.Type, ReqHubClaimTypes.PlanName, StringComparison.Ordinal));
            return claim?.Value;
        }

        public static string GetNormalizedPlanName(this ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(x => string.Equals(x.Type, ReqHubClaimTypes.NormalizedPlanName, StringComparison.Ordinal));
            return claim?.Value;
        }

        public static string GetPlanSku(this ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(x => string.Equals(x.Type, ReqHubClaimTypes.PlanSku, StringComparison.Ordinal));
            return claim?.Value;
        }

        public static string GetNormalizedPlanSku(this ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(x => string.Equals(x.Type, ReqHubClaimTypes.NormalizedPlanSku, StringComparison.Ordinal));
            return claim?.Value;
        }

        public static bool IsTrial(this ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(x => string.Equals(x.Type, ReqHubClaimTypes.IsTrial, StringComparison.Ordinal));
            return string.Equals(claim?.Value, "true", StringComparison.OrdinalIgnoreCase);
        }
    }
}
