namespace Kolokwium2.Dtos;

public class ClientDto
{
    public int IdClient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public List<SubscriptionDto>? Subscriptions { get; set; }
}
