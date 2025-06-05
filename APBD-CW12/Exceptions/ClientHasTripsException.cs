namespace APBD_CW12.Exceptions;

public class ClientHasTripsException:Exception
{
    public ClientHasTripsException(string? message) : base(message)
    {
    }
}