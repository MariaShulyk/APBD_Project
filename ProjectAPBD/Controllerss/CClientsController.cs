using Microsoft.AspNetCore.Mvc;
using ProjectAPBD.Services;
using ProjectAPBD.Errors;
using ProjectAPBD.ReqModels;

namespace ProjectAPBD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CClientsController(ICompanyClientService _companyClientService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> AddCClient([FromBody] AddCClientReqModel model)
    {
        try
        {
            var result = await _companyClientService.AddCClientAsync(model);
            return Created(string.Empty, null);
        }
        catch (BadRequestError e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCClient(int id, [FromBody] UpdateCClientReqModel model)
    {
        try
        {
            var result = await _companyClientService.UpdateCClientAsync(id, model);
            return Ok(result);
        }
        catch (NotFoundError e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestError ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientById(int id)
    {
        try
        {
            var result = await _companyClientService.GetCClientByIdAsync(id);
            return Ok(result);
        }
        catch (NotFoundError e)
        {
            return NotFound(e.Message);
        }
    }
}