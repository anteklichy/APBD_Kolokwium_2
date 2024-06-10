using Kolokwium2.Context;
using Kolokwium2.Dtos;
using Kolokwium2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly MyDbContext _context;

    public PaymentController(MyDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult> CreatePayment(CreatePaymentDto createPaymentDto)
    {
        var client = await _context.Clients.FindAsync(createPaymentDto.IdClient);
        if (client == null) return NotFound("Client not found");

        var subscription = await _context.Subscriptions.FindAsync(createPaymentDto.IdSubscription);
        if (subscription == null) return NotFound("Subscription not found");

        if (subscription.EndTime < DateTime.Now) return BadRequest("Subscription is not active");

        var lastSale = await _context.Sales
            .Where(s => s.IdClient == createPaymentDto.IdClient && s.IdSubscription == createPaymentDto.IdSubscription)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastSale == null || (DateTime.Now - lastSale.CreatedAt).TotalDays > (subscription.RenewalPeriod * 30))
        {
            return BadRequest("Payment not within subscription period");
        }

        var discounts = await _context.Discounts
            .Where(d => d.IdSubscription == createPaymentDto.IdSubscription && d.DateFrom <= DateTime.Now && d.DateTo >= DateTime.Now)
            .ToListAsync();

        var discount = discounts.OrderByDescending(d => d.Value).FirstOrDefault();

        var finalPayment = subscription.Price;
        if (discount != null)
        {
            finalPayment -= (subscription.Price * discount.Value / 100);
        }

        if (createPaymentDto.Payment != finalPayment) return BadRequest("Incorrect payment amount");

        var existingPayment = await _context.Payments
            .AnyAsync(p => p.IdClient == createPaymentDto.IdClient && p.IdSubscription == createPaymentDto.IdSubscription && p.Date > lastSale.CreatedAt);

        if (existingPayment) return BadRequest("Payment already made for this period");

        var newPayment = new Payment
        {
            IdClient = createPaymentDto.IdClient,
            IdSubscription = createPaymentDto.IdSubscription,
            Date = DateTime.Now,
            Amount = createPaymentDto.Payment
        };

        _context.Payments.Add(newPayment);
        await _context.SaveChangesAsync();

        var paymentDto = new PaymentDto
        {
            IdPayment = newPayment.IdPayment,
            Date = newPayment.Date,
            IdClient = newPayment.IdClient,
            IdSubscription = newPayment.IdSubscription,
            Amount = newPayment.Amount
        };

        return CreatedAtAction(nameof(GetPaymentById), new { id = paymentDto.IdPayment }, paymentDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDto>> GetPaymentById(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null) return NotFound();

        var paymentDto = new PaymentDto
        {
            IdPayment = payment.IdPayment,
            Date = payment.Date,
            IdClient = payment.IdClient,
            IdSubscription = payment.IdSubscription,
            Amount = payment.Amount
        };

        return Ok(paymentDto);
    }
}
