using Forkfully.Domain.Common.Models;
using Forkfully.Domain.User.ValueObjects;
using ErrorOr;

namespace Forkfully.Domain.User;

// The account/identity aggregate. A user may act as a Host and/or Guest — those
// are separate aggregates that reference the user by UserId.
public sealed class User : AggregateRoot<UserId, Guid>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }   // a salted PBKDF2 hash (see IPasswordHasher), never plaintext

    private User(
        UserId userId,
        string firstName,
        string lastName,
        string email,
        string password)
        : base(userId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }

#pragma warning disable CS8618
    // Parameterless constructor for EF Core / serialization.
    private User()
    {
    }
#pragma warning restore CS8618

    public static ErrorOr<User> Create(string firstName, string lastName, string email, string password)
    {
        return new User(
            UserId.CreateUnique(),
            firstName,
            lastName,
            email,
            password);
    }
}
