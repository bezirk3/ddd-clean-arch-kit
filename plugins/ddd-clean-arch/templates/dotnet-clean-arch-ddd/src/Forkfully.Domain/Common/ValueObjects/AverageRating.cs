using Forkfully.Domain.Common.Models;

namespace Forkfully.Domain.Common.ValueObjects;

// Shared across aggregates that carry a rating (Menu, Host, Guest).
public sealed class AverageRating : ValueObject
{
    // Private setters + parameterless ctor so EF can materialize the owned value object.
    public double Value { get; private set; }
    public int NumRatings { get; private set; }

    private AverageRating(double value, int numRatings)
    {
        Value = value;
        NumRatings = numRatings;
    }

    private AverageRating()
    {
    }

    public static AverageRating CreateNew(double value = 0, int numRatings = 0)
    {
        return new AverageRating(value, numRatings);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return NumRatings;
    }
}
