using Forkfully.Domain.Bill;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Bills.Queries.GetBill;

public record GetBillQuery(string BillId) : IRequest<ErrorOr<Bill>>;
