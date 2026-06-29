using Forkfully.Domain.Dinner;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Dinners.Queries.ListDinners;

public record ListDinnersQuery() : IRequest<ErrorOr<List<Dinner>>>;
