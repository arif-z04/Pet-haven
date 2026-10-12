using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetHaven.Api.Models;

[Table("customer_addresses")]
public class CustomerAddress
{
    [Key]
    [Column("id")]
    public ulong Id { get; set; }

    [Required]
    [Column("user_id")]
    public ulong UserId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("full_name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Column("phone")]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("division")]
    public string Division { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("district")]
    public string District { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("area")]
    public string Area { get; set; } = string.Empty;

    [Required]
    [Column("address_line")]
    public string AddressLine { get; set; } = string.Empty;

    [MaxLength(20)]
    [Column("postal_code")]
    public string? PostalCode { get; set; }

    [Column("is_default")]
    public bool IsDefault { get; set; }

    [MaxLength(20)]
    [Column("address_type")]
    public string AddressType { get; set; } = "home";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
}
