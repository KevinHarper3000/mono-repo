using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class UserClaimViewModel
{
    [ConcurrencyCheck]
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    [Key]
    public Guid Id { get; set; }

    [MaxLength(250)]
    [Required]
    public string Type { get; set; }

    public UserClaimViewModel UserClaim { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [MaxLength(250)]
    [Required]
    public string Value { get; set; }
}