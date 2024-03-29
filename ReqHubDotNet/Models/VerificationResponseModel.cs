﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ReqHub
{
    [ExcludeFromCodeCoverage]
    public class VerificationResponseModel
    {
        public string ClientId { get; set; }

        public string PlanName { get; set; }

        public string NormalizedPlanName { get; set; }

        public string PlanSku { get; set; }

        public string NormalizedPlanSku { get; set; }

        public bool IsTrial { get; set; }
    }
}
