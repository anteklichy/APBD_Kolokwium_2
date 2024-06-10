namespace Kolokwium2.Services;

using Repositories;
using Dtos;
using Exceptions;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ClientDto> GetClientWithSubscriptionsAsync(int id)
    {
        var client = await _clientRepository.GetClientByIdAsync(id);
        if (client == null)
        {
            throw new NotFoundException("Client not found");
        }

        var clientDto = new ClientDto
        {
            FirstName = client.FirstName,
            LastName = client.LastName,
            Email = client.Email,
            Phone = client.Phone,
            Subscriptions = client.Sales.Select(s => new SubscriptionDto
            {
                IdSubscription = s.IdSubscription,
                Name = s.Subscription.Name,
                TotalPaidAmount = client.Payments.Where(p => p.IdSubscription == s.IdSubscription).Sum(p => p.Amount)
            }).ToList()
        };

        return clientDto;
    }
}

public interface IClientService
{
    Task<ClientDto> GetClientWithSubscriptionsAsync(int id);
}