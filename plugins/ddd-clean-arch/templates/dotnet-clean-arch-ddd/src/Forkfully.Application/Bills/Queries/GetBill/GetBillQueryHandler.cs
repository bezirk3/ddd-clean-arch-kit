using Forkfully.Application.Common.Interfaces;
using Forkfully.Application.Common.Messaging;
using Forkfully.Domain.Bill;
using Forkfully.Domain.Bill.ValueObjects;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Forkfully.Application.Bills.Queries.GetBill;

public class GetBillQueryHandler : IRequestHandler<GetBillQuery, ErrorOr<Bill>>
{
    private readonly IApplicationDbContext _context;

    public GetBillQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Bill>> Handle(GetBillQuery query, CancellationToken cancellationToken)
    {
        var billId = BillId.Create(Guid.Parse(query.BillId));
        var bill = await _context.Bills.SingleOrDefaultAsync(b => b.Id == billId, cancellationToken);

        return bill is null
            ? Error.NotFound("Bill.NotFound", "Bill not found.")
            : bill;
    }
}
