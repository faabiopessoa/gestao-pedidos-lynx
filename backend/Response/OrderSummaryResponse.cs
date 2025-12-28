namespace GestaoPedidosLynx.Api.Response;

public class OrderSummaryResponse
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public int ItemsCount { get; set; }
    public int TotalCents { get; set; }
}