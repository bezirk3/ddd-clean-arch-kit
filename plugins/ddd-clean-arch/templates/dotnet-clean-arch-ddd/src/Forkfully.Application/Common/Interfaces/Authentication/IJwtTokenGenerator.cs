using Forkfully.Domain.User;

namespace Forkfully.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
