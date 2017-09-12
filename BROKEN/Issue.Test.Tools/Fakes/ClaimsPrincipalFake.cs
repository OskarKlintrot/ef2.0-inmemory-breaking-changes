using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Intranet.Test.Tools.Fakes
{
    public class ClaimsPrincipalFake : ClaimsPrincipal
    {
        public ClaimsPrincipalFake(params Claim[] claims)
            : base(new ClaimsIdentityFake(claims))
        {
            // Fake
        }
    }
}
