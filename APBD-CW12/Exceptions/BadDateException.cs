namespace cw12.Exceptions;

public class BadDateException:Exception
{
    public BadDateException(string? message) : base(message)
    {
    }
}