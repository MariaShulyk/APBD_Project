using System.ComponentModel.DataAnnotations;

namespace ProjectAPBD.ReqModels;

public class LoginReqModel
{
    [MaxLength(50)]
    public string Login { get; set; }
    
    [MaxLength(50)]
    public string Password { get; set; }
}