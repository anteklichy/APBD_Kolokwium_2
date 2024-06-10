namespace Kolokwium2.Repositories;

using Kolokwium2.Context;
using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

public class PaymentRepository : IPaymentRepository
{
    private readonly MyDbContext _context;

    public PaymentRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> CreatePaymentAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> GetPaymentByIdAsync(int id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<bool> PaymentExistsAsync(int idClient, int idSubscription, DateTime fromDate)
    {
        return await _context.Payments
            .AnyAsync(p => p.IdClient == idClient && p.IdSubscription == idSubscription && p.Date > fromDate);
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

public interface IPaymentRepository
{
    Task<Payment> CreatePaymentAsync(Payment payment);
    Task<Payment> GetPaymentByIdAsync(int id);
    Task<bool> PaymentExistsAsync(int idClient, int idSubscription, DateTime fromDate);
    Task<Subscription> GetSubscriptionByIdAsync(int id);
    Task<Sale> GetLastSaleAsync(int idClient, int idSubscription);
    Task<List<Discount>> GetActiveDiscountsAsync(int idSubscription);
}
