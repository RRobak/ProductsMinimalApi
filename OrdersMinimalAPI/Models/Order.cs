namespace OrdersMinimalAPI.Models;

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Address { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
}