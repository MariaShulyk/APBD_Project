using System.ComponentModel.DataAnnotations;

namespace ProjectAPBD.ReqModels;

public class MakePaymentReqModel
{
    [Required]
    public int ClientId { get; set; }

    [Required]
    public int ContractId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; }
}