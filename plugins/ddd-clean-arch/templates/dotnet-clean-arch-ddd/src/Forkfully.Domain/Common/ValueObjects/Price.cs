using Forkfully.Domain.Common.Models;

namespace Forkfully.Domain.Common.ValueObjects;

public sealed class Price : ValueObject
{
    // Private setters + parameterless ctor so EF can materialize the owned value object.
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private Price(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

#pragma warning disable CS8618
    private Price()
    {
    }
#pragma warning restore CS8618

    public static Price Create(decimal amount, string currency)
    {
        return new Price(amount, currency);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
