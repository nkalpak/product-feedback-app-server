using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OauthServer.Features.ProductRequest;

[Table("Comment")]
public class Comment
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public IdentityUser User { get; set; }
}