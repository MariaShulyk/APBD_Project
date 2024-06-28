using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectAPBD.ReqModels;
using ProjectAPBD.Services;

namespace ProjectAPBD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(IPaymentService _paymentService) : ControllerBase
{
    
    [HttpPost]
    public async Task<IActionResult> MakePayment([FromBody] MakePaymentReqModel model)
    {
        try
        {
            var payment = await _paymentService.MakePaymentAsync(model);
            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, payment);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentById(int id)
    {
        try
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            return Ok(payment);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}