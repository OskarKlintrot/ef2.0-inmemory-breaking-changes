using System;
using System.Collections.Generic;
using System.Text;

namespace Intranet.Web.Common.Factories
{
    public class DateTimeFactory : IDateTimeFactory
    {
        public DateTime DateTime => DateTime.Now;

        public DateTimeOffset DateTimeOffset => DateTimeOffset.Now;

        public DateTimeOffset DateTimeOffsetUtc => DateTimeOffset.UtcNow;
    }
}
