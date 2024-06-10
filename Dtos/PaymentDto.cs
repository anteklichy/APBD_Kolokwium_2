namespace Kolokwium2.Dtos;

public class PaymentDto
{
    public int IdPayment { get; set; }
    public DateTime Date { get; set; }
    public int IdClient { get; set; }
    public int IdSubscription { get; set; }
    public decimal Amount { get; set; }
}
