using Forkfully.Domain.Dinner;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Dinners.Queries.GetDinner;

public record GetDinnerQuery(string DinnerId) : IRequest<ErrorOr<Dinner>>;
