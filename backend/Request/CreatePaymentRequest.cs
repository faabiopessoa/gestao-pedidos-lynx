namespace GestaoPedidosLynx.Api.Request;

public class CreatePaymentRequest
{
    public int OrderId { get; set; }
    public string Method { get; set; } = "";
    public int AmountCents { get; set; }
}