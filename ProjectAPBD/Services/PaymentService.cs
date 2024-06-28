using ProjectAPBD.Context;
using ProjectAPBD.Models;
using Microsoft.EntityFrameworkCore;
using ProjectAPBD.Errors;
using ProjectAPBD.ReqModels;
using ProjectAPBD.ResModels;

namespace ProjectAPBD.Services;

public interface IPaymentService
{
    Task<PaymentResModel> MakePaymentAsync(MakePaymentReqModel model);
    Task<PaymentResModel> GetPaymentByIdAsync(int id);
}

public class PaymentService(ContextDB _context) : IPaymentService
{

    public async Task<PaymentResModel> MakePaymentAsync(MakePaymentReqModel model)
    {
        var contract = await _context.Contracts
            .FirstOrDefaultAsync(c => c.Id == model.ContractId && c.ClientId == model.ClientId);

        if (contract == null)
        {
            throw new NotFoundError("Contract was not found");
        }

        if (contract.IsPaid)
        {
            throw new AmountError("Contract is already fully paid");
        }

        if (model.PaymentDate > contract.EndDate)
        {
            throw new DateError("Payment date is beyond the contract end date");
        }

        var totalPaid = await _context.Payments
            .Where(p => p.ContractId == model.ContractId)
            .SumAsync(p => p.Amount);

        if (totalPaid + model.Amount > contract.Price)
        {
            throw new AmountError("Payment is bigger than the contract price");
        }

        var payment = new Payment
        {
            ClientId = model.ClientId,
            ContractId = model.ContractId,
            Amount = model.Amount,
            PaymentDate = model.PaymentDate
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        if (totalPaid + model.Amount == contract.Price)
        {
            contract.IsPaid = true;
            contract.IsSigned = true;
            await _context.SaveChangesAsync();
        }

        return new PaymentResModel
        {
            Id = payment.Id,
            ClientId = payment.ClientId,
            ContractId = payment.ContractId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate
        };
    }

    public async Task<PaymentResModel> GetPaymentByIdAsync(int id)
    {
        var payment = await _context.Payments
            .Include(p => p.Client)
            .Include(p => p.Contract)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (payment == null)
        {
            throw new NotFoundError("Payment was not found");
        }

        return new PaymentResModel
        {
            Id = payment.Id,
            ClientId = payment.ClientId,
            ContractId = payment.ContractId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate
        };
    }
}