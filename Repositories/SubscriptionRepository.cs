namespace Kolokwium2.Repositories;

using Kolokwium2.Context;
using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly MyDbContext _context;

    public SubscriptionRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Subscription> GetSubscriptionByIdAsync(int id)
    {
        return await _context.Subscriptions.FindAsync(id);
    }

    public async Task<Sale> GetLastSaleAsync(int idClient, int idSubscription)
    {
        return await _context.Sales
            .Where(s => s.IdClient == idClient && s.IdSubscription == idSubscription)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Discount>> GetActiveDiscountsAsync(int idSubscription)
    {
        return await _context.Discounts
            .Where(d => d.IdSubscription == idSubscription && d.DateFrom <= DateTime.Now && d.DateTo >= DateTime.Now)
            .ToListAsync();
    }
}

public interface ISubscriptionRepository
{
    Task<Subscription> GetSubscriptionByIdAsync(int id);
    Task<Sale> GetLastSaleAsync(int idClient, int idSubscription);
    Task<List<Discount>> GetActiveDiscountsAsync(int idSubscription);
}