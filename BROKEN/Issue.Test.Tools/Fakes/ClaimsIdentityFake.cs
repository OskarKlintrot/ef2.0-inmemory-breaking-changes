using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Intranet.Test.Tools.Fakes
{
    public class ClaimsIdentityFake : ClaimsIdentity
    {
        public ClaimsIdentityFake(params Claim[] claims)
            : base(claims)
        {
            // Empty
        }
    }
}
