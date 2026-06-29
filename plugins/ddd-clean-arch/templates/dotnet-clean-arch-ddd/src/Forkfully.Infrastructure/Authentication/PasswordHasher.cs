using System.Security.Cryptography;
using Forkfully.Application.Common.Interfaces.Authentication;

namespace Forkfully.Infrastructure.Authentication;

// Salted PBKDF2-SHA256, in-box (no third-party package). The hash is self-describing:
// "{iterations}.{base64(salt)}.{base64(subkey)}" — so the parameters travel with the
// stored value and can evolve without a migration. Verification is constant-time.
public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;        // 128-bit salt
    private const int KeySize = 32;         // 256-bit derived subkey
    private const int Iterations = 100_000;
    private const char Delimiter = '.';
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var subkey = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

        return string.Join(
            Delimiter,
            Iterations,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(subkey));
    }

    public bool Verify(string password, string hash)
    {
        // A malformed/legacy (e.g. plaintext) value has no delimiters — fail cleanly, never throw.
        var parts = hash.Split(Delimiter);
        if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[1]);
        var subkey = Convert.FromBase64String(parts[2]);

        var attempted = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, Algorithm, subkey.Length);
        return CryptographicOperations.FixedTimeEquals(attempted, subkey);
    }
}
