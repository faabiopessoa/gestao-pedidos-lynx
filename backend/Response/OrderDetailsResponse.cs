namespace GestaoPedidosLynx.Api.Response;

public class OrderDetailsResponse
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public int TotalCents { get; set; }
    public int PaidCents { get; set; }
    public int RemainingCents { get; set; }

    public List<OrderItemDetailsResponse> Items { get; set; } = new();
}