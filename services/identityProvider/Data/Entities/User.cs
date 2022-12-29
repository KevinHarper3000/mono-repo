using System.ComponentModel.DataAnnotations;
using SvcCommon.Abstract;

namespace Data.Entities;

public class User : IConcurrencyAware
{
    [Required]
    public bool Active { get; set; }

    public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();

    [Key]
    public Guid Id { get; set; }

    [MaxLength(200)]
    public string Password { get; set; }

    [MaxLength(200)]
    [Required]
    public string Subject { get; set; }

    [MaxLength(200)]
    public string Username { get; set; }

    [ConcurrencyCheck]
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
}