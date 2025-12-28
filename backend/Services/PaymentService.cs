using GestaoPedidosLynx.Api.Models;
using GestaoPedidosLynx.Api.Request;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidosLynx.Api.Services;

public class PaymentService
{
    private readonly AppDbContext _db;

    public PaymentService(AppDbContext db)
    {
        _db = db;
    }

    public int CreatePayment(CreatePaymentRequest request)
    {
        if (request.OrderId <= 0)
            throw new Exception("ID do Pedido inválido.");

        if (string.IsNullOrWhiteSpace(request.Method))
            throw new Exception("Método de pagamento é obrigatório.");

        if (request.AmountCents <= 0)
            throw new Exception("Valor do pagamento deve ser maior do que 0.");

        var order = _db.Orders
            .Include(o => o.Items)
            .Include(o => o.Payments)
            .FirstOrDefault(o => o.Id == request.OrderId);

        if (order == null)
            throw new Exception("Pedido não encontrado.");

        var totalCents = order.Items.Sum(i => i.Quantity * i.UnitPriceCents);

        var alreadyPaidCents = order.Payments.Sum(p => p.AmountCents);

        var payment = new Payment
        {
            OrderId = order.Id,
            Method = request.Method.Trim().ToUpper(),
            AmountCents = request.AmountCents,
            PaidAt = DateTime.UtcNow
        };

        _db.Payments.Add(payment);

        var newPaidTotal = alreadyPaidCents + request.AmountCents;
        if (newPaidTotal >= totalCents)
        {
            order.Status = "PAID";
        }

        _db.SaveChanges();

        return payment.Id;
    }
}