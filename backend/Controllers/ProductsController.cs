using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoPedidosLynx.Api.Models;

namespace GestaoPedidosLynx.Api.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] string? search,
        [FromQuery] string? category,
        [FromQuery] bool? onlyActive
    )
    {
        var query = _db.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(s));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            var c = category.Trim().ToLower();
            query = query.Where(p => p.Category.ToLower() == c);
        }

        if (onlyActive == true)
        {
            query = query.Where(p => p.Active);
        }

        var products = await query.OrderBy(p => p.Category).ThenBy(p => p.Name).ToListAsync();

        return Ok(products);
    }
}