using ProjectAPBD.Context;
using ProjectAPBD.Models;
using Microsoft.EntityFrameworkCore;
using ProjectAPBD.Errors;
using ProjectAPBD.ReqModels;
using ProjectAPBD.ResModels;

namespace ProjectAPBD.Services;

public interface IContractService
{
    Task<ContractResModel> MakeContractAsync(MakeContractReqModel model);
    Task<ContractResModel> GetContractByIdAsync(int id);
}

public class ContractService(ContextDB _context) : IContractService
{

    public async Task<ContractResModel> MakeContractAsync(MakeContractReqModel model)
    {
        var activeContract = await _context.Contracts
            .FirstOrDefaultAsync(c => c.ClientId == model.ClientId && c.SoftwareId == model.SoftwareId && c.IsActive);

        if (activeContract != null)
        {
            throw new ValidContractError("Client already had an active contract for this product");
        }
        
        if ((model.EndDate - model.StartDate).TotalDays < 3 || (model.EndDate - model.StartDate).TotalDays > 30)
        {
            throw new AmountError("The duration of the contract must be at least 3 days and no more than 30 days");
        }
        
        var software = await _context.Softwares
            .Include(s => s.Discounts)
            .FirstOrDefaultAsync(s => s.Id == model.SoftwareId);

        if (software == null)
        {
            throw new NotFoundError("Software was not found");
        }

        decimal discountPercentage = 0;
        var applicableDiscounts = software.Discounts
            .Where(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
            .OrderByDescending(d => d.Percentage)
            .ToList();

        if (applicableDiscounts.Any())
        {
            discountPercentage = applicableDiscounts.First().Percentage;
        }

        var previousContracts = await _context.Contracts
            .Where(c => c.ClientId == model.ClientId && c.IsSigned)
            .ToListAsync();

        if (previousContracts.Any())
        {
            discountPercentage+=5;
        }

        var basePrice = software.Price;
        var discountedPrice = basePrice - (basePrice * discountPercentage /100);

        var additionalSupportCost = 1000 * model.AdditionalSupportYears;
        var totalPrice = discountedPrice + additionalSupportCost;

        var contract = new Contract
        {
            ClientId = model.ClientId,
            SoftwareId = model.SoftwareId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Price = totalPrice,
            IsSigned = false,
            IsPaid = false,
            IsActive = true,
            AdditionalSupportYears = model.AdditionalSupportYears
        };

        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync();

        return new ContractResModel
        {
            Id = contract.Id,
            ClientId = contract.ClientId,
            SoftwareId = contract.SoftwareId,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            Price = contract.Price,
            IsSigned = contract.IsSigned,
            IsPaid = contract.IsPaid,
            IsActive = contract.IsActive,
            AdditionalSupportYears = contract.AdditionalSupportYears
        };
    }

    public async Task<ContractResModel> GetContractByIdAsync(int id)
    {
        var contract = await _context.Contracts
            .Include(c => c.Client)
            .Include(c => c.Software)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contract == null)
        {
            throw new NotFoundError("Contract was not found");
        }

        return new ContractResModel
        {
            Id = contract.Id,
            ClientId = contract.ClientId,
            SoftwareId = contract.SoftwareId,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            Price = contract.Price,
            IsSigned = contract.IsSigned,
            IsPaid = contract.IsPaid,
            IsActive = contract.IsActive,
            AdditionalSupportYears = contract.AdditionalSupportYears
        };
    }
}