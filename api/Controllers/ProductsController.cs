using api.Data;
using api.Data.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly BonusHackerDbContext _context;

    public ProductsController(BonusHackerDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id:guid}")]
    public async Task<ScrapedProduct?> GetById(Guid id)
    {
        return await _context.ScrapedProducts.FirstOrDefaultAsync(p => p.Id == id);
    }
}