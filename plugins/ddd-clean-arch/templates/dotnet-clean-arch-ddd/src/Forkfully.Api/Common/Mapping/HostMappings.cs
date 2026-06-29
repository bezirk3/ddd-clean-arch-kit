using Forkfully.Application.Hosts.Commands.CreateHost;
using Forkfully.Contracts.Hosts;
using DomainHost = Forkfully.Domain.Host.Host;

namespace Forkfully.Api.Common.Mapping;

public static class HostMappings
{
    public static CreateHostCommand ToCommand(this CreateHostRequest request) =>
        new(request.UserId, request.FirstName, request.LastName, request.ProfileImage);

    public static HostResponse ToResponse(this DomainHost host) =>
        new(
            host.Id.Value.ToString(),
            host.UserId.Value.ToString(),
            host.FirstName,
            host.LastName,
            host.ProfileImage,
            host.AverageRating.Value,
            host.MenuIds.Select(id => id.Value.ToString()).ToList(),
            host.DinnerIds.Select(id => id.Value.ToString()).ToList());
}
