using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Dinner;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.Dinners.Queries.ListDinners;

public class ListDinnersQueryHandler : IRequestHandler<ListDinnersQuery, ErrorOr<List<Dinner>>>
{
    private readonly IApplicationDbContext _context;

    public ListDinnersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<List<Dinner>>> Handle(ListDinnersQuery query, CancellationToken cancellationToken)
    {
        return await _context.Dinners.ToListAsync(cancellationToken);
    }
}
