using cw12.Models;
using cw12.Models.DTOs;

namespace cw12.Services;

public interface ITripsService
{
        Task<TripsDTO> GetTrips(int pageNumber, int pageSize);
        Task AddClientToTrip(int tripId, ClientToTripDTO clientToTripDTO);
}