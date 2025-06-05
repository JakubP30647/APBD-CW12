namespace APBD_CW12.Exceptions;

public class NotFoundException: Exception
{
    public NotFoundException(string? message) : base(message)
    {
    }
}