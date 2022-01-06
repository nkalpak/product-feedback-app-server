using System.ComponentModel.DataAnnotations;
using OauthServer.Features.Auth;

namespace OauthServer.Features.ProductRequest;

public class ProductRequestDto
{
    public ProductRequestDto(ProductRequest productRequest)
    {
        Category = productRequest.Category;
        Comments = productRequest.Comments.Select(x => new CommentDto
        {
            Content = x.Content,
            Id = x.Id,
            User = new IdentityUserDto
            {
                Id = new Guid(x.User.Id),
                Username = x.User.UserName,
            },
        }).ToList();
        Description = productRequest.Description;
        Id = productRequest.Id;
        Status = productRequest.Status;
        Title = productRequest.Title;
        Upvotes = productRequest.Upvotes;
    }

    [Required] public Guid Id { get; set; }

    [Required] public string Title { get; set; }

    [Required] public ProductRequestCategory Category { get; set; } = ProductRequestCategory.Feature;

    [Required] public int Upvotes { get; set; } = 0;

    [Required] public ProductRequestStatus Status { get; set; } = ProductRequestStatus.Suggestion;

    public string Description { get; set; }

    [Required] public List<CommentDto> Comments { get; set; } = new();

    [Required] public bool HasCurrentUserUpvoted { get; set; }
}