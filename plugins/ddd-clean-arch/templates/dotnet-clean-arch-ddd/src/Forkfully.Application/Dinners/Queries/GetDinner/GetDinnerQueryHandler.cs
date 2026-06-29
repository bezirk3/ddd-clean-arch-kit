using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Dinner;
using Forkfully.Domain.Dinner.ValueObjects;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.Dinners.Queries.GetDinner;

public class GetDinnerQueryHandler : IRequestHandler<GetDinnerQuery, ErrorOr<Dinner>>
{
    private readonly IApplicationDbContext _context;

    public GetDinnerQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Dinner>> Handle(GetDinnerQuery query, CancellationToken cancellationToken)
    {
        var dinnerId = DinnerId.Create(Guid.Parse(query.DinnerId));
        var dinner = await _context.Dinners.SingleOrDefaultAsync(d => d.Id == dinnerId, cancellationToken);

        return dinner is null
            ? Error.NotFound("Dinner.NotFound", "Dinner not found.")
            : dinner;
    }
}
