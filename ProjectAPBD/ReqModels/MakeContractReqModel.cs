using System.ComponentModel.DataAnnotations;

namespace ProjectAPBD.ReqModels;

public class MakeContractReqModel
{
    [Required]
    public int ClientId { get; set; }

    [Required]
    public int SoftwareId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public decimal Price { get; set; }

    public int AdditionalSupportYears { get; set; }
}