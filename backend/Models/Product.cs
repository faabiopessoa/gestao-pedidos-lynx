namespace GestaoPedidosLynx.Api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Category { get; set; } = null!;
    public int PriceCents { get; set; }
    public bool Active { get; set; } = true;
}
