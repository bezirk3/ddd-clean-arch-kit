using Forkfully.Domain.Menu;
using ErrorOr;
using Forkfully.Application.Common.Messaging;

namespace Forkfully.Application.Menus.Queries.GetMenu;

public record GetMenuQuery(string HostId, string MenuId) : IRequest<ErrorOr<Menu>>;
