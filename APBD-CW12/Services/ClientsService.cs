using System.Data.Common;
using System.Transactions;
using cw12.Data;
using cw12.Exceptions;
using cw12.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace cw12.Services;

public class ClientsService:IClientsService
{
    private readonly Cw12Context _context;

    public ClientsService(Cw12Context context)
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
                throw new HasTripsException($"Client with id: {id}, has trips and can't be deleted");
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