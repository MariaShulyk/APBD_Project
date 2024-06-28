using System.ComponentModel.DataAnnotations;

namespace ProjectAPBD.ReqModels;

public class UpdateCClientReqModel
{
    [Required]
    [MaxLength(100)]
    public string Address { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; }

    [Required]
    [MaxLength(100)]
    public string CompanyName { get; set; }
}