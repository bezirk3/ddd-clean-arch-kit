using Forkfully.Contracts.Bills;
using DomainBill = Forkfully.Domain.Bill.Bill;

namespace Forkfully.Api.Common.Mapping;

public static class BillMappings
{
    public static BillResponse ToResponse(this DomainBill bill) =>
        new(
            bill.Id.Value.ToString(),
            bill.DinnerId.Value.ToString(),
            bill.GuestId.Value.ToString(),
            bill.Amount.Amount,
            bill.Amount.Currency);
}
