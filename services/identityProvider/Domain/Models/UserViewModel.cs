using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class UserViewModel
{
    [Required]
    public bool Active { get; set; }

    public ICollection<UserClaimViewModel> Claims { get; set; } = new List<UserClaimViewModel>();

    [ConcurrencyCheck]
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    [Key]
    public Guid Id { get; set; }

    [MaxLength(200)]
    public string Password { get; set; }

    [MaxLength(200)]
    [Required]
    public string Subject { get; set; }

    [MaxLength(200)]
    public string Username { get; set; }
}