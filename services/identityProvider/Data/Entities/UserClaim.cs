using System.ComponentModel.DataAnnotations;
using SvcCommon.Abstract;

namespace Data.Entities;

public class UserClaim : IConcurrencyAware
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(250)]
    [Required]
    public string Type { get; set; }

    public User User { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [MaxLength(250)]
    [Required]
    public string Value { get; set; }

    [ConcurrencyCheck]
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
}