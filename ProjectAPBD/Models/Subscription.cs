using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAPBD.Models;

[Table("Subscriptions")]
public class Subscription
{
    [Key]
    [Column("Id")]
    public int SubscriptionId { get; set; }
    
    [Required]
    [ForeignKey(nameof(Client))]
    [Column("ClientId")]
    public int ClientId { get; set; }
    
    [Required]
    [ForeignKey(nameof(Software))]
    [Column("SoftwareId")]
    public int SoftwareId { get; set; }
    
    [Required]
    [Column("Name")]
    public string Name { get; set; }
    
    [Required]
    [Column("Renewal")]
    public int RenewalPeriodMonths { get; set; }
    
    [Required]
    [Column("Price")]
    public double Price { get; set; }
    
    [Required]
    [Column("StartDate")]
    public DateTime StartDate { get; set; }
    
    public Client Client { get; set; }
    
    public Software Software { get; set; }
}
