namespace Kolokwium2.Dtos;

public class CreatePaymentDto
{
    public int IdClient { get; set; }
    public int IdSubscription { get; set; }
    public decimal Payment { get; set; }
}
