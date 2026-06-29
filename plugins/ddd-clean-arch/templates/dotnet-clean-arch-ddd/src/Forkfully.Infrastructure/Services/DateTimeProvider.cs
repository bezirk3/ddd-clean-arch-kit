using Forkfully.Application.Common.Interfaces.Services;

namespace Forkfully.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
