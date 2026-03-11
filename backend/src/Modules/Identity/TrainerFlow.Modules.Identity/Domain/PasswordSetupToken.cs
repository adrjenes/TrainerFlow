namespace TrainerFlow.Modules.Identity.Domain;

public sealed class PasswordSetupToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = null!;
    public DateTime CreatedUtc { get; private set; }
    public DateTime ExpiresUtc { get; private set; }
    public DateTime? UsedUtc { get; private set; }

    private PasswordSetupToken() { }

    public PasswordSetupToken(Guid userId, string tokenHash, DateTime expiresUtc)
    {
        if (userId == Guid.Empty) throw new ArgumentException("UserId is required.", nameof(userId));

        if (string.IsNullOrWhiteSpace(tokenHash)) throw new ArgumentException("Token hash is required.", nameof(tokenHash));

        if (expiresUtc <= DateTime.UtcNow) throw new ArgumentException("Expiration date must be in the future.", nameof(expiresUtc));

        Id = Guid.CreateVersion7();
        UserId = userId;
        TokenHash = tokenHash;
        CreatedUtc = DateTime.UtcNow;
        ExpiresUtc = expiresUtc;
    }

    public bool IsExpired => DateTime.UtcNow > ExpiresUtc;
    public bool IsUsed => UsedUtc.HasValue;

    public void MarkAsUsed()
    {
        if (IsUsed) throw new InvalidOperationException("Password setup token already used.");

        if (IsExpired) throw new InvalidOperationException("Password setup token expired.");

        UsedUtc = DateTime.UtcNow;
    }
}