using Forkfully.Domain.Common.Models;

namespace Forkfully.Domain.Dinner.Events;

// Raised when a Dinner is created. Handled across the aggregate boundary: the dinner's
// Menu records the new dinner's id (DinnerCreatedEventHandler), published on SaveChanges.
public record DinnerCreated(Dinner Dinner) : IDomainEvent;
