using ErrorOr;

namespace Forkfully.Domain.Common.Errors;

public static partial class Errors
{
    public static class MenuReview
    {
        public static Error InvalidRating => Error.Validation(
            code: "MenuReview.InvalidRating",
            description: "A menu review rating must be between 1 and 5.");
    }
}
