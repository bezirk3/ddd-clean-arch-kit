namespace Forkfully.Application.Common.Interfaces.Authentication;

// Hashing of user passwords. The Application owns the contract; Infrastructure owns the
// algorithm (a salted PBKDF2 hash) — the same inward-pointing seam as IJwtTokenGenerator.
public interface IPasswordHasher
{
    // Produces a self-describing hash string (salt + parameters embedded) for storage.
    string Hash(string password);

    // Constant-time verification of a plaintext password against a stored hash.
    bool Verify(string password, string hash);
}
