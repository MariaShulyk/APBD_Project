using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAPBD.Models;

public class IndividualClient : Client
{
    [Required]
    [MaxLength(50)]
    [Column("Name")]
    public string Name { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("Surname")]
    public string Surname { get; set; }

    [Required]
    [MaxLength(11)]
    [Column("Pesel")]
    public string Pesel { get; set; }
}