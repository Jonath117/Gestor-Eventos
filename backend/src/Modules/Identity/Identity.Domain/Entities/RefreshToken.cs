namespace Identity.Domain.Entities;

public class RefreshToken
{
    public string Token { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public DateTime ExpiryDate { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? ReplacedByToken { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
    public bool IsActive => !IsRevoked && !IsExpired;

    private RefreshToken() { } // EF Core

    internal RefreshToken(string token, Guid userId, DateTime expiryDate)
    {
        Token = token;
        UserId = userId;
        ExpiryDate = expiryDate;
        CreatedAt = DateTime.UtcNow;
    }

    internal void Revoke(string? replacedByToken = null)
    {
        IsRevoked = true;
        ReplacedByToken = replacedByToken;
    }
}