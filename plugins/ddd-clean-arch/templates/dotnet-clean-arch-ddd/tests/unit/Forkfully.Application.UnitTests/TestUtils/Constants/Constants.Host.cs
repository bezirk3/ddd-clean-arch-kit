using Forkfully.Domain.Host.ValueObjects;

namespace Forkfully.Application.UnitTests.TestUtils;

public static partial class Constants
{
    public static class Host
    {
        public static readonly HostId Id = HostId.Create(Guid.NewGuid());
    }
}
