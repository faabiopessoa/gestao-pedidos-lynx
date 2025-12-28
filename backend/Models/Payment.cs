namespace GestaoPedidosLynx.Api.Models;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public string Method { get; set; } = null!;
    public int AmountCents { get; set; }
    public DateTime? PaidAt { get; set; }
}
