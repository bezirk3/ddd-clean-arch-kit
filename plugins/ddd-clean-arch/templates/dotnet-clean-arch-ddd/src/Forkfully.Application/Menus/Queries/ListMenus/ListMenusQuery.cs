using Forkfully.Domain.Menu;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Menus.Queries.ListMenus;

public record ListMenusQuery(string HostId) : IRequest<ErrorOr<List<Menu>>>;
