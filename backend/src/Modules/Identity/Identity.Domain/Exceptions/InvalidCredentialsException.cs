namespace Identity.Domain.Exceptions;

public class InvalidCredentialsException : IdentityException
{
    public InvalidCredentialsException() : base("Invalid email or password.")
    {
    }
}
