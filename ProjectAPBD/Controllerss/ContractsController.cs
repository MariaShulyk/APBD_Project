using Microsoft.AspNetCore.Mvc;
using ProjectAPBD.ReqModels;
using ProjectAPBD.Services;

namespace ProjectAPBD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractsController(IContractService _contractService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> MakeContract([FromBody] MakeContractReqModel model)
    {
        try
        {
            var contract = await _contractService.MakeContractAsync(model);
            return CreatedAtAction(nameof(GetContractById), new { id = contract.Id }, contract);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContractById(int id)
    {
        try
        {
            var contract = await _contractService.GetContractByIdAsync(id);
            return Ok(contract);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}