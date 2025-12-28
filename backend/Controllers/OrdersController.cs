using GestaoPedidosLynx.Api.Request;
using GestaoPedidosLynx.Api.Response;
using GestaoPedidosLynx.Api.Services;
using GestaoPedidosLynx.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPedidosLynx.Api.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;
    private readonly AppDbContext _db;

    public OrdersController(OrderService orderService, AppDbContext db)
    {
        _orderService = orderService;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderSummaryResponse
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer.Name,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                ItemsCount = o.Items.Sum(i => i.Quantity),
                TotalCents = o.Items.Sum(i => i.Quantity * i.UnitPriceCents)
            }).ToListAsync();

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _db.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .Include(o => o.Payments)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound(new { error = "Pedido não encontrado." });

        var totalCents = order.Items.Sum(i => i.Quantity * i.UnitPriceCents);
        var paidCents = order.Payments.Sum(p => p.AmountCents);

        var response = new OrderDetailsResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer != null ? order.Customer.Name : "",
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            TotalCents = totalCents,
            PaidCents = paidCents,
            RemainingCents = totalCents - paidCents,
            Items = order.Items.Select(i => new OrderItemDetailsResponse
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPriceCents = i.UnitPriceCents
            }).ToList()
        };

        return Ok(response);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateOrderRequest request)
    {
        try
        {
            var orderId = _orderService.CreateOrder(request);
            return Ok(new { order_id = orderId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
