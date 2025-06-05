using System.Net;
using APBD_CW12.Models;
using APBD_CW12.Data;
using APBD_CW12.Exceptions;
using APBD_CW12.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace APBD_CW12.Services;

public class TripsService : ITripsService
{
    private readonly ApbdCw12Context _context;

    public TripsService(ApbdCw12Context context)
    {
        _context = context;
    }


    public async Task<TripsDTO> GetTrips(int pageNumber, int pageSize)
    {
        var allPages = (int)Math.Ceiling((double)await _context.Trips.CountAsync() / pageSize);
        
        var tripsDTO = await _context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountryDTO
                {
                    Name = c.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDTO
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
                
            })
            .ToListAsync();
        
        return new TripsDTO()
        {
            PageNum = pageNumber,
            PageSize = pageSize,
            AllPages = allPages,
            Trips = tripsDTO
        };
    }

    public async Task AddClientToTrip(int tripId, ClientToTripDTO clientToTripDTO)
    {
        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var client = _context.Clients.FirstOrDefault(c => c.Pesel == clientToTripDTO.Pesel);
            var trip = _context.Trips
                .Include(t => t.ClientTrips)
                .FirstOrDefault(t => t.IdTrip == tripId);
            if (client == null)
            {
                throw new NotFoundException("Client with pesel " + clientToTripDTO.Pesel + " does not exist");
            }

            if (trip == null)
            {
                throw new NotFoundException("Trip with id " + tripId + " does not exist");
            }

            if (trip.DateFrom < DateTime.Now)
            {
                throw new BadRequestException("Trip with id " + tripId + " is in the past");
            }

            if (trip.ClientTrips.Any(ct => ct.IdClient == client.IdClient))
            {
                throw new ClientTripException(
                    $"Client with id {client.IdClient} is already in trip with id {tripId}");
            }

            var clientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = trip.IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientToTripDTO.PaymentDate
            };
            await _context.ClientTrips.AddAsync(clientTrip);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}