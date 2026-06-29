using Forkfully.Domain.Common.Models;

namespace Forkfully.Domain.Menu.Events;

public record MenuCreated(Menu Menu) : IDomainEvent;
