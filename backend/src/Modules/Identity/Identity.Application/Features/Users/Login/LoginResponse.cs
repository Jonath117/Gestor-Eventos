namespace Identity.Application.Features.Users.Login;

public record LoginResponse(string AccessToken, string RefreshToken);
