using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using OauthServer.Helpers;

namespace OauthServer.Features.ProductRequest;

[Table("ProductRequestVote")]
public class ProductRequestVote : BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string UserId { get; set; }
    public IdentityUser User { get; set; }

    public Guid ProductRequestId { get; set; }
    public ProductRequest ProductRequest { get; set; }

    public ProductRequestVoteDirection Direction { get; set; }
}

public enum ProductRequestVoteDirection {
    Upvote = 1,
    Downvote = -1
}