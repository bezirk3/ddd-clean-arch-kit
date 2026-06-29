using Forkfully.Application.Authentication.Common;
using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Interfaces.Authentication;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Common.Errors;
using Forkfully.Domain.User;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.Authentication.Queries.Login;

public class LoginQueryHandler
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IApplicationDbContext _context;

    public LoginQueryHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordHasher passwordHasher,
        IApplicationDbContext context)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
        _context = context;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginQuery query,
        CancellationToken cancellationToken)
    {
        // 1. Validate the user exists
        if (await _context.Users.SingleOrDefaultAsync(user => user.Email == query.Email, cancellationToken) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // 2. Validate the password is correct (constant-time verify against the stored hash)
        if (!_passwordHasher.Verify(query.Password, user.Password))
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // 3. Create JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
