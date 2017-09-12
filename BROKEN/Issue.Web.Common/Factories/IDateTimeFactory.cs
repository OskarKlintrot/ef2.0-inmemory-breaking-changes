using System;

namespace Intranet.Web.Common.Factories
{
    public interface IDateTimeFactory
    {
        DateTime DateTime { get; }
        DateTimeOffset DateTimeOffset { get; }
        DateTimeOffset DateTimeOffsetUtc { get; }
    }
}