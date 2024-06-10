namespace Kolokwium2.Services;

using Kolokwium2.Repositories;
using Kolokwium2.Dtos;
using Kolokwium2.Models;
using Kolokwium2.Exceptions;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public PaymentService(IPaymentRepository paymentRepository, IClientRepository clientRepository, ISubscriptionRepository subscriptionRepository)
    {
        _paymentRepository = paymentRepository;
        _clientRepository = clientRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<Payment> MakePaymentAsync(PaymentDto paymentDto)
    {
        var client = await _clientRepository.GetClientByIdAsync(paymentDto.IdClient);
        if (client == null)
        {
            throw new NotFoundException("Client not found");
        }

        var subscription = await _subscriptionRepository.GetSubscriptionByIdAsync(paymentDto.IdSubscription);
        if (subscription == null)
        {
            throw new NotFoundException("Subscription not found");
        }

        if (subscription.EndTime < DateTime.Now)
        {
            throw new InvalidOperationException("Subscription has ended");
        }

        var lastSale = await _subscriptionRepository.GetLastSaleAsync(paymentDto.IdClient, paymentDto.IdSubscription);
        if (lastSale == null || (DateTime.Now - lastSale.CreatedAt).TotalDays > (subscription.RenewalPeriod * 30))
        {
            throw new InvalidOperationException("Payment not within subscription period");
        }

        var existingPayment = await _paymentRepository.PaymentExistsAsync(paymentDto.IdClient, paymentDto.IdSubscription, lastSale.CreatedAt);
        if (existingPayment)
        {
            throw new InvalidOperationException("Payment already made for this period");
        }

        var discounts = await _subscriptionRepository.GetActiveDiscountsAsync(paymentDto.IdSubscription);
        var discount = discounts.OrderByDescending(d => d.Value).FirstOrDefault();

        var finalPayment = subscription.Price;
        if (discount != null)
        {
            finalPayment -= (subscription.Price * discount.Value / 100);
        }

        if (paymentDto.Amount != finalPayment)
        {
            throw new InvalidOperationException("Incorrect payment amount");
        }

        var payment = new Payment
        {
            IdClient = paymentDto.IdClient,
            IdSubscription = paymentDto.IdSubscription,
            Date = DateTime.Now,
            Amount = paymentDto.Amount
        };

        return await _paymentRepository.CreatePaymentAsync(payment);
    }
}

public interface IPaymentService
{
    Task<Payment> MakePaymentAsync(PaymentDto paymentDto);
}
