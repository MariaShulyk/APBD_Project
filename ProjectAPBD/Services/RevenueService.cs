using ProjectAPBD.Context;
using Microsoft.EntityFrameworkCore;
using ProjectAPBD.Errors;
using ProjectAPBD.ReqModels;
using ProjectAPBD.ResModels;

namespace ProjectAPBD.Services;

public interface IRevenueService
{
    Task<RevenueResModel> CalculateRevenueAsync(ComputeRevenueReqModel model);
    Task<decimal> CalculateExpectedRevenueAsync();
}
public class RevenueService(ContextDB _context) : IRevenueService
{

    public async Task<RevenueResModel> CalculateRevenueAsync(ComputeRevenueReqModel model)
    {
        if (model == null)
        {
            throw new BadRequestError("Request model cannot be null");
        }
        
        decimal totalRevenue = 0;

        if (model.SoftwareId.HasValue)
        {
            var contracts = await _context.Contracts
                .Where(c => c.SoftwareId == model.SoftwareId.Value && c.IsSigned && c.IsPaid)
                .ToListAsync();

            if (!contracts.Any())
            {
                throw new NotFoundError($"No signed and paid contracts found for Software {model.SoftwareId.Value}");
            }
            
            totalRevenue = contracts.Sum(c => c.Price);
        }
        else
        {
            var contracts = await _context.Contracts
                .Where(c => c.IsSigned && c.IsPaid)
                .ToListAsync();

            if (!contracts.Any())
            {
                throw new NotFoundError("No signed and paid contracts found");
            }
            
            totalRevenue = contracts.Sum(c => c.Price);
        }

        return new RevenueResModel
        {
            TotalRevenue = totalRevenue,
            TotalRevenueInPLN = totalRevenue,
            Currency = "PLN"
        };
    }
    
    public async Task<decimal> CalculateExpectedRevenueAsync()
    {
        var signedRevenue = await _context.Contracts
            .Where(c => c.IsSigned && c.IsPaid)
            .SumAsync(c => c.Price);

        var expectedRevenue = await _context.Contracts
            .Where(c => !c.IsSigned)
            .SumAsync(c => c.Price);

        return signedRevenue + expectedRevenue;
    }
}