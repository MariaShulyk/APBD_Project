using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectAPBD.ReqModels;
using ProjectAPBD.ResModels;
using ProjectAPBD.Services;

namespace ProjectAPBD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RevenueController(IRevenueService _revenueService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ComputeRevenue([FromBody] ComputeRevenueReqModel model)
    {
        try
        {
            var revenue = await _revenueService.CalculateRevenueAsync(model);
            return Ok(revenue);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("calculate-expected")]
    public async Task<IActionResult> ComputeExpectedRevenue()
    {
        try
        {
            var expectedRevenue = await _revenueService.CalculateExpectedRevenueAsync();
            return Ok(new ComputeRevenueResModel { Revenue = expectedRevenue });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}