using ErrorOr;

namespace Forkfully.Domain.Common.Errors;

public static partial class Errors
{
    public static class Dinner
    {
        public static Error CannotReserveMoreSpotsThanAvailable => Error.Validation(
            code: "Dinner.CannotReserveMoreSpotsThanAvailable",
            description: "Cannot reserve more spots than are available for this dinner.");

        public static Error EndBeforeStart => Error.Validation(
            code: "Dinner.EndBeforeStart",
            description: "A dinner's end time must be after its start time.");

        public static Error InvalidGuestCount => Error.Validation(
            code: "Dinner.InvalidGuestCount",
            description: "A dinner must allow at least one guest.");
    }
}
