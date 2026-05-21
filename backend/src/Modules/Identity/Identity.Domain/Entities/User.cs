namespace Identity.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    private readonly List<RefreshToken> _refreshTokens = new();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private readonly List<OrganizationUser> _organizationUsers = new();
    public IReadOnlyCollection<OrganizationUser> OrganizationUsers => _organizationUsers.AsReadOnly();

    private User() { } // EF Core

    public User(Guid id, string email, string passwordHash)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    public static User Create(string email, string passwordHash)
    {
        return new User(Guid.NewGuid(), email, passwordHash);
    }

    public void AddRefreshToken(string token, DateTime expiryDate)
    {
        _refreshTokens.Add(new RefreshToken(token, Id, expiryDate));
    }

    public void JoinOrganization(Guid organizationId, string role)
    {
        if (!_organizationUsers.Any(x => x.OrganizationId == organizationId))
        {
            _organizationUsers.Add(new OrganizationUser(organizationId, Id, role));
        }
    }

    public void RevokeRefreshToken(string token, string? replacedByToken = null)
    {
        var refreshToken = _refreshTokens.SingleOrDefault(x => x.Token == token);
        refreshToken?.Revoke(replacedByToken);
    }
}
