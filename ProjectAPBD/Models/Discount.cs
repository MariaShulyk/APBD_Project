using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAPBD.Models;

[Table("Discounts")]
public class Discount
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("Name")]
    public string Name { get; set; }

    [Required]
    [Column("Percentage")]
    public decimal Percentage { get; set; }
    
    [Column("Duration")]
    public int Duration { get; set; }

    [Required]
    [Column("Start")]
    public DateTime StartDate { get; set; }

    [Required]
    [Column("End")]
    public DateTime EndDate { get; set; }

    public ICollection<Software> Softwares { get; set; }
}