namespace GestaoPedidosLynx.Api.Request;

public class CreateOrderItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}