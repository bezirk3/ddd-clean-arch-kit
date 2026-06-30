# Slice templates (per layer)

Verbatim shapes lifted from the reference solution. Substitute placeholders:

- `<Name>` — root namespace (e.g. `Acme.Orders`)
- `<X>` — aggregate, PascalCase singular (e.g. `Product`)
- `<Xs>` — plural, used for DbSet/route/folder (e.g. `Products`, route `/products`)
- `<xs>` — lowercase route segment (e.g. `products`)

The example below models a minimal aggregate with one value-typed field (`Name`), one numeric
invariant field (`Price`), and one reference to another aggregate (`OwnerId`). Adapt the fields.

---

## 1. Domain — `src/<Name>.Domain/<X>/ValueObjects/<X>Id.cs`

```csharp
using <Name>.Domain.Common.Models;

namespace <Name>.Domain.<X>.ValueObjects;

public sealed class <X>Id : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private <X>Id(Guid value) => Value = value;

    public static <X>Id CreateUnique() => new(Guid.CreateVersion7());   // GUID v7 — time-ordered

    public static <X>Id Create(Guid value) => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
```

## 2. Domain — `src/<Name>.Domain/<X>/Events/<X>Created.cs`

```csharp
using <Name>.Domain.Common.Models;
using DomainX = <Name>.Domain.<X>.<X>;

namespace <Name>.Domain.<X>.Events;

public record <X>Created(DomainX <X>) : IDomainEvent;
```

## 3. Domain — `src/<Name>.Domain/Common/Errors/Errors.<X>.cs`

```csharp
using ErrorOr;

namespace <Name>.Domain.Common.Errors;

public static partial class Errors
{
    public static class <X>
    {
        public static Error NotFound => Error.NotFound(
            code: "<X>.NotFound", description: "<X> not found.");

        public static Error InvalidPrice => Error.Validation(
            code: "<X>.InvalidPrice", description: "<X> price must be greater than zero.");
    }
}
```

## 4. Domain — `src/<Name>.Domain/<X>/<X>.cs`

```csharp
using <Name>.Domain.Common.Errors;
using <Name>.Domain.Common.Models;
using <Name>.Domain.<X>.Events;
using <Name>.Domain.<X>.ValueObjects;
using ErrorOr;

namespace <Name>.Domain.<X>;

public sealed class <X> : AggregateRoot<<X>Id, Guid>
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }

    private <X>(<X>Id id, string name, decimal price) : base(id)
    {
        Name = name;
        Price = price;
        CreatedDateTime = DateTime.UtcNow;
        UpdatedDateTime = DateTime.UtcNow;
    }

#pragma warning disable CS8618
    private <X>() { }                       // EF materialization ctor
#pragma warning restore CS8618

    public static ErrorOr<<X>> Create(string name, decimal price)
    {
        if (price <= 0)                     // domain invariant (defense-in-depth)
        {
            return Errors.<X>.InvalidPrice;
        }

        var <x> = new <X>(<X>Id.CreateUnique(), name, price);
        <x>.AddDomainEvent(new <X>Created(<x>));
        return <x>;
    }
}
```

---

## 5. Application — `Common/Interfaces/IApplicationDbContext.cs` (add a line)

```csharp
DbSet<<Name>.Domain.<X>.<X>> <Xs> { get; }
```

## 6. Application — `<Xs>/Commands/Create<X>/Create<X>Command.cs`

```csharp
using <Name>.Application.Common.Messaging;
using <Name>.Domain.<X>;
using ErrorOr;

namespace <Name>.Application.<Xs>.Commands.Create<X>;

public record Create<X>Command(string Name, decimal Price) : IRequest<ErrorOr<<X>>>;
```

## 7. Application — `Create<X>CommandHandler.cs`

```csharp
using <Name>.Application.Common.Interfaces;
using <Name>.Application.Common.Messaging;
using <Name>.Domain.<X>;
using ErrorOr;

namespace <Name>.Application.<Xs>.Commands.Create<X>;

public class Create<X>CommandHandler : IRequestHandler<Create<X>Command, ErrorOr<<X>>>
{
    private readonly IApplicationDbContext _context;
    public Create<X>CommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<ErrorOr<<X>>> Handle(Create<X>Command command, CancellationToken cancellationToken)
    {
        var result = <X>.Create(command.Name, command.Price);
        if (result.IsError)
        {
            return result.Errors;
        }

        var <x> = result.Value;
        _context.<Xs>.Add(<x>);
        await _context.SaveChangesAsync(cancellationToken);
        return <x>;
    }
}
```

## 8. Application — `Create<X>CommandValidator.cs`

```csharp
using FluentValidation;

namespace <Name>.Application.<Xs>.Commands.Create<X>;

public class Create<X>CommandValidator : AbstractValidator<Create<X>Command>
{
    public Create<X>CommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
```

## 9. Application — `<Xs>/Queries/Get<X>/Get<X>Query.cs` + Handler

```csharp
public record Get<X>Query(string <X>Id) : IRequest<ErrorOr<<X>>>;
```

```csharp
using Microsoft.EntityFrameworkCore;   // for SingleOrDefaultAsync
// ...
public async Task<ErrorOr<<X>>> Handle(Get<X>Query query, CancellationToken cancellationToken)
{
    var id = <X>Id.Create(Guid.Parse(query.<X>Id));
    var <x> = await _context.<Xs>.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    return <x> is null ? Errors.<X>.NotFound : <x>;
}
```

## 10. Application — DI registration (`Configuration/RequestHandlers.cs`, and `EventHandlers.cs` if cross-aggregate)

```csharp
// in AddRequestHandlers(), add to the chain:
.AddRequestHandler<Create<X>Command, ErrorOr<<X>>, Create<X>CommandHandler>()
.AddRequestHandler<Get<X>Query, ErrorOr<<X>>, Get<X>QueryHandler>()

// in AddEventHandlers(), only if a handler reacts to <X>Created:
services.AddScoped<IDomainEventHandler<<X>Created>, <X>CreatedEventHandler>();
```

---

## 11. Infrastructure — `Persistence/Configurations/<X>Configurations.cs`

```csharp
using <Name>.Domain.<X>;
using <Name>.Domain.<X>.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace <Name>.Infrastructure.Persistence.Configurations;

public class <X>Configurations : IEntityTypeConfiguration<<X>>
{
    public void Configure(EntityTypeBuilder<<X>> builder)
    {
        builder.ToTable("<Xs>");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => <X>Id.Create(value));

        builder.Property(e => e.Name).HasMaxLength(100);
        builder.Property(e => e.Price).HasPrecision(18, 2);

        // For a reference to another aggregate, value-convert its typed id:
        // builder.Property(e => e.OwnerId).HasConversion(id => id.Value, v => OwnerId.Create(v));
        // For an owned value object: builder.OwnsOne(e => e.SomeVo);
        // For an owned collection of value objects / local entities: builder.OwnsMany(...) + backing field.
    }
}
```

## 12. Infrastructure — `Persistence/ApplicationDbContext.cs` (add a line)

```csharp
public DbSet<<Name>.Domain.<X>.<X>> <Xs> { get; set; } = null!;
```

Then create the migration:

```bash
dotnet ef migrations add Add<X> -p src/<Name>.Infrastructure -s src/<Name>.Api
```

---

## 13. Contracts — `<Xs>/Create<X>Request.cs` and `<X>Response.cs`

```csharp
namespace <Name>.Contracts.<Xs>;

public record Create<X>Request(string Name, decimal Price);

public record <X>Response(string Id, string Name, decimal Price);
```

## 14. Api — `Common/Mapping/<X>Mappings.cs`

```csharp
using <Name>.Application.<Xs>.Commands.Create<X>;
using <Name>.Contracts.<Xs>;
using DomainX = <Name>.Domain.<X>.<X>;

namespace <Name>.Api.Common.Mapping;

public static class <X>Mappings
{
    public static Create<X>Command ToCommand(this Create<X>Request request) =>
        new(request.Name, request.Price);

    public static <X>Response ToResponse(this DomainX <x>) =>
        new(<x>.Id.Value.ToString(), <x>.Name, <x>.Price);   // unwrap typed id via .Value
}
```

## 15. Api — `Endpoints/<Xs>Endpoints.cs`

```csharp
using <Name>.Api.Common.Http;
using <Name>.Api.Common.Mapping;
using <Name>.Application.Common.Messaging;
using <Name>.Application.<Xs>.Commands.Create<X>;
using <Name>.Application.<Xs>.Queries.Get<X>;
using <Name>.Contracts.<Xs>;
using <Name>.Domain.<X>;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace <Name>.Api.Endpoints;

public static class <Xs>Endpoints
{
    public static IEndpointRouteBuilder Map<X>Endpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/<xs>");
        group.MapPost("", Create<X>);
        group.MapGet("/{<x>Id}", Get<X>);
        return routes;
    }

    public static async Task<IResult> Create<X>(
        Create<X>Request request,
        [FromServices] IRequestHandler<Create<X>Command, ErrorOr<<X>>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);
        return result.Match<IResult>(
            <x> => TypedResults.Created($"/<xs>/{<x>.Id.Value}", <x>.ToResponse()),  // 201 + Location
            errors => ErrorResults.From(httpContext, errors));
    }

    public static async Task<IResult> Get<X>(
        string <x>Id,
        [FromServices] IRequestHandler<Get<X>Query, ErrorOr<<X>>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new Get<X>Query(<x>Id), cancellationToken);
        return result.Match<IResult>(
            <x> => TypedResults.Ok(<x>.ToResponse()),
            errors => ErrorResults.From(httpContext, errors));
    }
}
```

## 16. Api — register in the aggregator `Endpoints/EndpointRouteBuilderExtensions.cs`

```csharp
routes.Map<X>Endpoints();
```

---

## 17. Tests — `tests/unit/<Name>.Domain.UnitTests/<Xs>/<X>Tests.cs`

```csharp
using <Name>.Domain.<X>;
using <Name>.Domain.<X>.Events;
using Xunit;

namespace <Name>.Domain.UnitTests.<Xs>;

public class <X>Tests
{
    [Fact]
    public void Create_WhenValid_ReturnsAggregateAndRaisesCreatedEvent()
    {
        var result = <X>.Create("widget", 9.99m);

        Assert.False(result.IsError);
        Assert.Single(result.Value.DomainEvents, e => e is <X>Created);
    }

    [Fact]
    public void Create_WhenPriceNotPositive_ReturnsInvalidPriceError()
    {
        var result = <X>.Create("widget", 0m);

        Assert.True(result.IsError);
        Assert.Equal("<X>.InvalidPrice", result.FirstError.Code);
    }
}
```

## 18. Tests — `tests/unit/<Name>.Application.UnitTests/<Xs>/Commands/Create<X>/Create<X>CommandValidatorTests.cs`

```csharp
using <Name>.Application.<Xs>.Commands.Create<X>;
using FluentValidation.TestHelper;   // or assert on .Validate(...).IsValid to match the house style
using Xunit;

namespace <Name>.Application.UnitTests.<Xs>.Commands.Create<X>;

public class Create<X>CommandValidatorTests
{
    private readonly Create<X>CommandValidator _validator = new();

    [Theory]
    [InlineData("", 1)]      // empty name
    [InlineData("ok", 0)]    // non-positive price
    public void Validate_WhenInvalid_Fails(string name, decimal price)
    {
        var result = _validator.Validate(new Create<X>Command(name, price));
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_WhenValid_Passes()
    {
        var result = _validator.Validate(new Create<X>Command("widget", 9.99m));
        Assert.True(result.IsValid);
    }
}
```

> The reference solution uses plain xUnit `Assert` (FluentAssertions was removed). Match whatever
> the target solution uses — check an existing test first.
