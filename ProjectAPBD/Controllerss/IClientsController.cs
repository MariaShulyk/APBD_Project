using Microsoft.AspNetCore.Mvc;
using ProjectAPBD.Errors;
using ProjectAPBD.ReqModels;
using ProjectAPBD.Services;

namespace ProjectAPBD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IClientsController(IIndividualClientService _individualClientService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> AddIClient([FromBody] AddIClientReqModel model)
    {
        try
        {
            var result = await _individualClientService.AddIndividualClientAsync(model);
            return Created(string.Empty, null);
        }
        catch (BadRequestError e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIClient(int id, [FromBody] UpdateIClientReqModel model)
    {
        try
        {
            var result = await _individualClientService.UpdateIndividualClientAsync(id, model);
            return Ok(result);
        }
        catch (NotFoundError e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestError e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIClientById(int id)
    {
        try
        {
            var result = await _individualClientService.GetIndividualClientByIdAsync(id);
            return Ok(result);
        }
        catch (NotFoundError e)
        {
            return NotFound(e.Message);
        }
    }
}