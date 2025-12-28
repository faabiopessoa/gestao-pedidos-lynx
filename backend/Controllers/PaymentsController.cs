using GestaoPedidosLynx.Api.Request;
using GestaoPedidosLynx.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPedidosLynx.Api.Controllers;

[ApiController]
[Route("payments")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentsController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreatePaymentRequest request)
    {
        try
        {
            var paymentId = _paymentService.CreatePayment(request);
            return Ok(new { payment_id = paymentId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
} 