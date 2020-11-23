using System;
using System.Collections.Generic;
using System.Text;

namespace ReqHub
{
    public static class ReqHubClaimTypes
    {
        public const string ClientId = "https://reqhub.io/identity/claims/clientid";

        public const string PlanName = "https://reqhub.io/identity/claims/planname";

        public const string NormalizedPlanName = "https://reqhub.io/identity/claims/planname/normalized";

        public const string PlanSku = "https://reqhub.io/identity/claims/plansku";

        public const string NormalizedPlanSku = "https://reqhub.io/identity/claims/plansku/normalized";

        public const string IsTrial = "https://reqhub.io/identity/claims/trial";
    }
}
