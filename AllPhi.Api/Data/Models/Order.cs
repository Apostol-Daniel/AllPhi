namespace AllPhi.Api.Data.Models;

public class Order
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreationDate { get; set; }
    public bool IsCancelled { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; } 
}