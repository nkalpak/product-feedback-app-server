using System.ComponentModel.DataAnnotations;

namespace OauthServer.Features.ProductRequest;

public class ProductRequestCreateRequest
{
    [Required] public string Title { get; set; }
    
    [Required] public ProductRequestCategory Category { get; set; }
    
    [Required] public ProductRequestStatus Status { get; set; }

    public string Description { get; set; }
}