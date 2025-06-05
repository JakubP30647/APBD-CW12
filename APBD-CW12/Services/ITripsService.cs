using APBD_CW12.Models;
using APBD_CW12.Models.DTOs;

namespace APBD_CW12.Services;

public interface ITripsService
{
        Task<TripsDTO> GetTrips(int pageNumber, int pageSize);
        Task AddClientToTrip(int tripId, ClientToTripDTO clientToTripDTO);
}