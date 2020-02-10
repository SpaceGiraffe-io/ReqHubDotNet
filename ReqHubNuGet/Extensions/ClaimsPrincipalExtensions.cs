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
    }
}
