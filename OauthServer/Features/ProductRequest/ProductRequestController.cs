using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OauthServer.Features.Auth;
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

    [HttpPost("vote/{id:guid}")]
    public async Task<ActionResult> Vote(Guid id, ProductRequestVoteDirection direction)
    {
        // Handle only upvotes for now
        if (direction == ProductRequestVoteDirection.Downvote)
        {
            return Ok();
        }

        var user = (User) HttpContext.Items["User"];
        var existingVote =
            await _context.ProductRequestVote.FirstOrDefaultAsync(vote =>
                user != null && vote.ProductRequestId == id && vote.UserId == user.Id && vote.Direction == direction);

        // We only support upvotes, so if a vote already exists we count it as clearing the upvote.
        if (existingVote != null)
        {
            _context.ProductRequestVote.Remove(existingVote);
            await _context.SaveChangesAsync();
            return Ok();
        }

        var productRequest = await _context.ProductRequest.FirstOrDefaultAsync(x => x.Id == id);
        if (productRequest == null) return BadRequest("That product request does not exist.");

        await _context.ProductRequestVote.AddAsync(new ProductRequestVote()
        {
            ProductRequestId = id,
            UserId = user?.Id,
            Direction = direction,
        });

        await _context.SaveChangesAsync();

        return Ok();
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
    public async Task<ActionResult<IEnumerable<ProductRequestDto>>> GetAll(ProductRequestSortBy sortBy)
    {
        var result = await _context.ProductRequest
            .Include(x => x.ProductRequestVotes)
            .ToListAsync();
        var user = (User) HttpContext.Items["User"];

        return Ok(result
            .Select(productRequest =>
            {
                var hasVoted = productRequest
                    .ProductRequestVotes
                    .FirstOrDefault(vote => vote.UserId == user.Id) != null;

                return new ProductRequestDto(productRequest)
                {
                    HasCurrentUserUpvoted = hasVoted,
                    Upvotes = productRequest.ProductRequestVotes.Count
                };
            })
            .OrderByDescending(productRequest => sortBy == ProductRequestSortBy.MostUpvotes
                ? productRequest.Upvotes
                : productRequest.Comments.Count));
    }

    [HttpGet("{id:required}")]
    public async Task<ActionResult<ProductRequestDto>> Get(string id)
    {
        var result = await _context.ProductRequest
            .Include(x => x.ProductRequestVotes)
            .FirstOrDefaultAsync(x => x.Id.ToString() == id);
        if (result == null) return NotFound($"Couldn't find product request with id {id}.");

        var user = (User) HttpContext.Items["User"];
        Console.WriteLine($"RESULT {result.ProductRequestVotes}");
        var hasVoted = result.ProductRequestVotes.FirstOrDefault(vote => vote.UserId == user.Id) != null;

        return Ok(new ProductRequestDto(result)
        {
            HasCurrentUserUpvoted = hasVoted,
            Upvotes = result.ProductRequestVotes.Count
        });
    }
}

public enum ProductRequestSortBy
{
    MostUpvotes = 0,
    MostComments = 1
}