using System.Data.Common;
using System.Transactions;
using APBD_CW12.Data;
using APBD_CW12.Exceptions;
using APBD_CW12.Models;
using APBD_CW12.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace APBD_CW12.Services;

public class ClientsService:IClientsService
{
    private readonly ApbdCw12Context _context;

    public ClientsService(ApbdCw12Context context)
    {
        _context = context;
    }
    
    public async Task Delete(int id)
    {
        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var c = _context.Clients
                .Include(c => c.ClientTrips)
                .FirstOrDefault(c => c.IdClient == id);
            if (c == null)
            {
                throw new NotFoundException($"Client with id: {id} not found");
            }

            if (c.ClientTrips.Count >= 1)
            {
                throw new ClientHasTripsException($"Client with id: {id}, has trips and can't be deleted");
            }

            _context.Clients.Remove(c);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }

        
    }
}