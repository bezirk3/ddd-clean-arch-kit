using Forkfully.Infrastructure.Authentication;

namespace Forkfully.Infrastructure.UnitTests.Authentication;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void Hash_ProducesSaltedValue_NeverThePlaintext()
    {
        var hash = _hasher.Hash("Password123!");

        Assert.NotEqual("Password123!", hash);
        // "{iterations}.{base64(salt)}.{base64(subkey)}"
        Assert.Equal(3, hash.Split('.').Length);
    }

    [Fact]
    public void Hash_IsSalted_SamePasswordHashesDifferently()
    {
        var first = _hasher.Hash("Password123!");
        var second = _hasher.Hash("Password123!");

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Verify_WithCorrectPassword_ReturnsTrue()
    {
        var hash = _hasher.Hash("Password123!");

        Assert.True(_hasher.Verify("Password123!", hash));
    }

    [Fact]
    public void Verify_WithWrongPassword_ReturnsFalse()
    {
        var hash = _hasher.Hash("Password123!");

        Assert.False(_hasher.Verify("WrongPassword", hash));
    }

    [Fact]
    public void Verify_WithLegacyPlaintextValue_ReturnsFalseInsteadOfThrowing()
    {
        // A pre-hashing row holds a bare plaintext string (no delimiters) — fail cleanly.
        Assert.False(_hasher.Verify("Password123!", "Password123!"));
    }
}
