using Forkfully.Application.Authentication.Commands.Register;
using Forkfully.Application.Authentication.Common;
using Forkfully.Application.Authentication.Queries.Login;
using Forkfully.Application.Bills.Queries.GetBill;
using Forkfully.Application.Common.Messaging;
using Forkfully.Application.Dinners.Commands.CreateDinner;
using Forkfully.Application.Dinners.Commands.ReserveDinner;
using Forkfully.Application.Dinners.Queries.GetDinner;
using Forkfully.Application.Dinners.Queries.ListDinners;
using Forkfully.Application.Guests.Commands.CreateGuest;
using Forkfully.Application.Guests.Queries.GetGuest;
using Forkfully.Application.Hosts.Commands.CreateHost;
using Forkfully.Application.Hosts.Queries.GetHost;
using Forkfully.Application.Menus.Commands.CreateMenu;
using Forkfully.Application.Menus.Queries.GetMenu;
using Forkfully.Application.Menus.Queries.ListMenus;
using Forkfully.Application.MenuReviews.Commands.CreateMenuReview;
using Forkfully.Application.MenuReviews.Queries.GetMenuReview;
using Forkfully.Domain.Bill;
using Forkfully.Domain.Dinner;
using Forkfully.Domain.Guest;
using Forkfully.Domain.Host;
using Forkfully.Domain.Menu;
using Forkfully.Domain.MenuReview;
using ErrorOr;
using Microsoft.Extensions.DependencyInjection;

namespace Forkfully.Application.Configuration;

public static class RequestHandlers
{
    public static IServiceCollection AddRequestHandlers(this IServiceCollection services)
    {
        // Each request handler is registered with its decorator stack (validation +
        // logging) by AddRequestHandler. No assembly scan, no pipeline — the wiring
        // is explicit and you can step straight from the controller into the handler.
        services
            .AddRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>, RegisterCommandHandler>()
            .AddRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>, LoginQueryHandler>()
            .AddRequestHandler<CreateHostCommand, ErrorOr<Host>, CreateHostCommandHandler>()
            .AddRequestHandler<GetHostQuery, ErrorOr<Host>, GetHostQueryHandler>()
            .AddRequestHandler<CreateGuestCommand, ErrorOr<Guest>, CreateGuestCommandHandler>()
            .AddRequestHandler<GetGuestQuery, ErrorOr<Guest>, GetGuestQueryHandler>()
            .AddRequestHandler<CreateMenuCommand, ErrorOr<Menu>, CreateMenuCommandHandler>()
            .AddRequestHandler<GetMenuQuery, ErrorOr<Menu>, GetMenuQueryHandler>()
            .AddRequestHandler<ListMenusQuery, ErrorOr<List<Menu>>, ListMenusQueryHandler>()
            .AddRequestHandler<CreateDinnerCommand, ErrorOr<Dinner>, CreateDinnerCommandHandler>()
            .AddRequestHandler<GetDinnerQuery, ErrorOr<Dinner>, GetDinnerQueryHandler>()
            .AddRequestHandler<ListDinnersQuery, ErrorOr<List<Dinner>>, ListDinnersQueryHandler>()
            .AddRequestHandler<ReserveDinnerCommand, ErrorOr<Dinner>, ReserveDinnerCommandHandler>()
            .AddRequestHandler<CreateMenuReviewCommand, ErrorOr<MenuReview>, CreateMenuReviewCommandHandler>()
            .AddRequestHandler<GetMenuReviewQuery, ErrorOr<MenuReview>, GetMenuReviewQueryHandler>()
            .AddRequestHandler<GetBillQuery, ErrorOr<Bill>, GetBillQueryHandler>();

            return services;
    }
}