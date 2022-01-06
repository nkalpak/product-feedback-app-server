using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OauthServer.Features.ProductRequest;

[Table("ProductRequestVote")]
public class ProductRequestVote
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required] public IdentityUser User { get; set; }

    [Required] public ProductRequest ProductRequest { get; set; }

    [Required] public bool Upvote { get; set; }

    [Required] public DateTime Timestamp { get; set; }
}