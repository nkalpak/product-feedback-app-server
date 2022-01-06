using System.ComponentModel.DataAnnotations;
using OauthServer.Features.Auth;

namespace OauthServer.Features.ProductRequest;

public class CommentDto
{
    [Required] public Guid Id { get; set; }

    [Required] public string Content { get; set; }

    [Required] public IdentityUserDto User { get; set; }
}