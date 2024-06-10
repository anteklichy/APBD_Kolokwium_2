using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Repositories;

using Models;
using Context;

public class ClientRepository : IClientRepository
{
    private readonly MyDbContext _context;

    public ClientRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Client> GetClientByIdAsync(int id)
    {
        return await _context.Clients.Include(c => c.Sales)
            .ThenInclude(s => s.Subscription)
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.IdClient == id);
    }
}

public interface IClientRepository
{
    Task<Client> GetClientByIdAsync(int id);
}
