using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OauthServer.Helpers;

namespace OauthServer.Features.ProductRequest;

[Authorize]
[ApiController]
[Route("/api/product_request")]
public class ProductRequestController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductRequestController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<ProductRequestDto>> Create(ProductRequestCreateRequest request)
    {
        var result = await _context.ProductRequest.AddAsync(new ProductRequest
        {
            Category = request.Category,
            Description = request.Description,
            Status = request.Status,
            Title = request.Title,
        });

        await _context.SaveChangesAsync();

        return Ok(new ProductRequestDto(result.Entity));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductRequestDto>>> GetAll()
    {
        var result = await _context.ProductRequest.ToListAsync();
        return Ok(result.Select(x => new ProductRequestDto(x)));
    }

    [HttpGet("{id:required}")]
    public async Task<ActionResult<ProductRequestDto>> Get(string id)
    {
        var result = await _context.ProductRequest.FirstOrDefaultAsync(x => x.Id.ToString() == id);
        if (result == null) return NotFound($"Couldn't find product request with id {id}.");

        return Ok(new ProductRequestDto(result));
    }
}