using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAPBD.Models;

[Table("Clients")]
public abstract class Client
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("Address")]
    public string Address { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("Email")]
    public string Email { get; set; }

    [Required]
    [MaxLength(15)]
    [Column("PhoneNumber")]
    public string PhoneNumber { get; set; }

    [Column("IsDeleted")]
    public bool IsDeleted { get; set; }
    
    [Required]
    [Column("Type")]
    public string Type { get; set; }
    
    // [Required]
    // [MaxLength(100)]
    // [Column("CompanyName")]
    // public string CompanyName { get; set; }
    //
    // [Required]
    // [MaxLength(10)]
    // [Column("KRS")]
    // public string KRS { get; set; }
    
    // [Required]
    // [MaxLength(50)]
    // [Column("Name")]
    // public string Name { get; set; }
    //
    // [Required]
    // [MaxLength(50)]
    // [Column("Surname")]
    // public string Surname { get; set; }

    public ICollection<Contract> Contracts { get; set; }
    public ICollection<Payment> Payments { get; set; }
}