namespace APBD_CW12.Exceptions;

public class BadRequestException:Exception
{
    public BadRequestException(string? message) : base(message)
    {
    }
}