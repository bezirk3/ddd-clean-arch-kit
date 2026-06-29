using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Host;
using Forkfully.Domain.User.ValueObjects;
using ErrorOr;

namespace Forkfully.Application.Hosts.Commands.CreateHost;

public class CreateHostCommandHandler : IRequestHandler<CreateHostCommand, ErrorOr<Host>>
{
    private readonly IApplicationDbContext _context;

    public CreateHostCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Host>> Handle(CreateHostCommand command, CancellationToken cancellationToken)
    {
        var result = Host.Create(
            UserId.Create(Guid.Parse(command.UserId)),
            command.FirstName,
            command.LastName,
            command.ProfileImage);

        if (result.IsError)
        {
            return result.Errors;
        }

        var host = result.Value;
        _context.Hosts.Add(host);
        await _context.SaveChangesAsync(cancellationToken);

        return host;
    }
}
