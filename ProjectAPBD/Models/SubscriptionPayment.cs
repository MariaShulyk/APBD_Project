using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAPBD.Models;

[Table("SubscriptionPayments")]
public class SubscriptionPayment
{
    [Key]
    [Column("Id")]
    public int SubscriptionPaymentId { get; set; }
    
    [Required]
    [ForeignKey(nameof(Subscription))]
    [Column("SubscriptionId")]
    public int SubscriptionId { get; set; }
    
    [Required]
    [Column("Date")]
    public DateTime PaymentDate { get; set; }
    
    [Required]
    [Column("Amount")]
    public double Amount { get; set; }
    
    public Subscription Subscription { get; set; }
}