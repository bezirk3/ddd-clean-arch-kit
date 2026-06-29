using Forkfully.Application.Authentication.Common;
using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Interfaces.Authentication;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Common.Errors;
using Forkfully.Domain.User;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.Authentication.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IApplicationDbContext _context;

    public RegisterCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordHasher passwordHasher,
        IApplicationDbContext context)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
        _context = context;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Validate the user does not already exist
        if (await _context.Users.SingleOrDefaultAsync(user => user.Email == command.Email, cancellationToken) is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        // 2. Create user (generate unique id, hash the password) and persist to DB
        var result = User.Create(
            command.FirstName,
            command.LastName,
            command.Email,
            _passwordHasher.Hash(command.Password));

        if (result.IsError)
        {
            return result.Errors;
        }

        var user = result.Value;
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        // 3. Create JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
