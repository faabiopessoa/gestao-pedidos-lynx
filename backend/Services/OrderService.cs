using GestaoPedidosLynx.Api.Request;
using GestaoPedidosLynx.Api.Models;

namespace GestaoPedidosLynx.Api.Services;

public class OrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }

    public int CreateOrder(CreateOrderRequest request)
    {
        if (request.Items == null || request.Items.Count == 0)
            throw new Exception("Pedido precisa ter pelo menos 1 item.");

        if (request.Items.Any(i => i.Quantity <= 0))
            throw new Exception("Quantidade deve ser maior do que 0.");

        if (!_db.Customers.Any(c => c.Id == request.CustomerId))
            throw new Exception("Cliente não encontrado.");

        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = _db.Products.Where(p => productIds.Contains(p.Id)).ToList();

        if (products.Count != productIds.Count)
            throw new Exception("Um ou mais produtos não foram encontrados.");

        if (products.Any(p => !p.Active))
            throw new Exception("Existe produto inativo no pedido.");

        var order = new Order
        {
            CustomerId = request.CustomerId,
            Status = "NEW",
            CreatedAt = DateTime.UtcNow
        };

        foreach (var item in request.Items)
        {
            var p = products.First(x => x.Id == item.ProductId);

            order.Items.Add(new OrderItem
            {
                ProductId = p.Id,
                Quantity = item.Quantity,
                UnitPriceCents = p.PriceCents
            });
        }

        _db.Orders.Add(order);
        _db.SaveChanges();

        return order.Id;
    }
}