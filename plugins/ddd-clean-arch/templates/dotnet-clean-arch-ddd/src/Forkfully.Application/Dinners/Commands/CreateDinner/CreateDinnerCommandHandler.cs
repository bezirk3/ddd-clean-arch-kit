using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Common.ValueObjects;
using Forkfully.Domain.Dinner;
using Forkfully.Domain.Dinner.ValueObjects;
using Forkfully.Domain.Host.ValueObjects;
using Forkfully.Domain.Menu.ValueObjects;
using ErrorOr;

namespace Forkfully.Application.Dinners.Commands.CreateDinner;

public class CreateDinnerCommandHandler : IRequestHandler<CreateDinnerCommand, ErrorOr<Dinner>>
{
    private readonly IApplicationDbContext _context;

    public CreateDinnerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Dinner>> Handle(CreateDinnerCommand command, CancellationToken cancellationToken)
    {
        var result = Dinner.Create(
            command.Name,
            command.Description,
            command.StartDateTime,
            command.EndDateTime,
            command.MaxGuestCount,
            Price.Create(command.Price, command.Currency),
            HostId.Create(Guid.Parse(command.HostId)),
            MenuId.Create(Guid.Parse(command.MenuId)),
            Location.Create(command.LocationName, command.LocationAddress, command.Latitude, command.Longitude));

        if (result.IsError)
        {
            return result.Errors;
        }

        var dinner = result.Value;
        _context.Dinners.Add(dinner);
        await _context.SaveChangesAsync(cancellationToken);

        return dinner;
    }
}
