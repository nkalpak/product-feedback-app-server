using System.ComponentModel.DataAnnotations;

namespace OauthServer.Helpers;

public class BaseEntity
{
    [Required]
    public DateTime DateCreated { get; set; }

    [Required]
    public DateTime DateUpdated { get; set; }
}