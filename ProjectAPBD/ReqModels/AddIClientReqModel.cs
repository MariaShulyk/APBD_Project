using System.ComponentModel.DataAnnotations;

namespace ProjectAPBD.ReqModels;

public class AddIClientReqModel
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
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(50)]
    public string Surname { get; set; }

    [Required]
    [MaxLength(11)]
    public string Pesel { get; set; }
}