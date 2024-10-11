namespace AllPhi.Api.Middleware.Exceptions;

public class DuplicateEmailException : Exception
{
    public DuplicateEmailException(string message) : base(message) { }
}