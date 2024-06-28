using ProjectAPBD.Context;
using ProjectAPBD.Models;
using ProjectAPBD.Errors;
using ProjectAPBD.ReqModels;
using ProjectAPBD.ResModels;

namespace ProjectAPBD.Services;

public interface ICompanyClientService
{
    Task<CClientResModel> AddCClientAsync(AddCClientReqModel model);
    Task<CClientResModel> UpdateCClientAsync(int id, UpdateCClientReqModel model);
    Task<CClientResModel> GetCClientByIdAsync(int id);
}

public class CClientService(ContextDB _context) : ICompanyClientService
{

    public async Task<CClientResModel> AddCClientAsync(AddCClientReqModel model)
    {
        var client = new CompanyClient
        {
            CompanyName = model.CompanyName,
            KRS = model.KRS,
            Address = model.Address,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Type = "Company",
            IsDeleted = false
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return new CClientResModel
        {
            Id = client.Id,
            Address = client.Address,
            Email = client.Email,
            Phone = client.PhoneNumber,
            ClientType = client.Type,
            CompanyName = client.CompanyName,
            KRS = client.KRS
        };
    }

    public async Task<CClientResModel> UpdateCClientAsync(int id, UpdateCClientReqModel model)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client == null || client.Type != "Company")
        {
            throw new NotFoundError($"Client with id {id} was not found");
        }

        var companyClient = client as CompanyClient;

        companyClient.CompanyName = model.CompanyName;
        companyClient.Address = model.Address;
        companyClient.Email = model.Email;
        companyClient.PhoneNumber = model.PhoneNumber;

        await _context.SaveChangesAsync();

        return new CClientResModel
        {
            Id = companyClient.Id,
            Address = companyClient.Address,
            Email = companyClient.Email,
            Phone = companyClient.PhoneNumber,
            ClientType = companyClient.Type,
            CompanyName = companyClient.CompanyName,
            KRS = companyClient.KRS
        };
    }

    public async Task<CClientResModel> GetCClientByIdAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client == null || client.Type != "Company")
        {
            throw new NotFoundError($"Client with id {id} was not found");
        }

        var companyClient = client as CompanyClient;

        return new CClientResModel
        {
            Id = companyClient.Id,
            Address = companyClient.Address,
            Email = companyClient.Email,
            Phone = companyClient.PhoneNumber,
            ClientType = companyClient.Type,
            CompanyName = companyClient.CompanyName,
            KRS = companyClient.KRS
        };
    }
}