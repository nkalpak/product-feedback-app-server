using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OauthServer.Features.ProductRequest;

[Table("ProductRequest")]
public class ProductRequest
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required] public string Title { get; set; }

    [Required] public ProductRequestCategory Category { get; set; } = ProductRequestCategory.Feature;

    [Required] public int Upvotes { get; set; } = 0;

    [Required] public ProductRequestStatus Status { get; set; } = ProductRequestStatus.Suggestion;

    public string Description { get; set; }

    [Required] public List<Comment> Comments { get; set; } = new();
}

public enum ProductRequestCategory
{
    Feature = 1,
    Enhancement = 2,
    Ui = 3,
    Ux = 4,
    Bug = 5
}

public enum ProductRequestStatus
{
    Suggestion = 1,
    Planned = 2,
    InProgress = 3,
    Live = 4,
}