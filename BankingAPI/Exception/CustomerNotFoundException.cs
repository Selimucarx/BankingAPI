namespace BankingAPI.Exception;

public class CustomerNotFoundException : System.Exception
{
    public CustomerNotFoundException(Guid customerId)
        : base($"Customer with ID {customerId} not found.")
    {
        CustomerId = customerId;
    }

    public Guid CustomerId { get; }
}