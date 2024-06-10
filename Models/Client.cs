namespace Kolokwium2.Models;

public class Client
{
    public int IdClient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public ICollection<Sale> Sales { get; set; }
    public ICollection<Payment> Payments { get; set; }
}