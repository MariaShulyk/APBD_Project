using ProjectAPBD.Context;
using ProjectAPBD.Models;
using ProjectAPBD.Errors;
using ProjectAPBD.ReqModels;
using ProjectAPBD.ResModels;

namespace ProjectAPBD.Services;

public interface IIndividualClientService
{
    Task<IClientResModel> AddIndividualClientAsync(AddIClientReqModel model);
    Task<IClientResModel> UpdateIndividualClientAsync(int id, UpdateIClientReqModel model);
    Task<IClientResModel> GetIndividualClientByIdAsync(int id);
}

public class IClientService(ContextDB _context) : IIndividualClientService
{

    public async Task<IClientResModel> AddIndividualClientAsync(AddIClientReqModel model)
    {
        var client = new IndividualClient
        {
            Name = model.Name,
            Surname = model.Surname,
            Pesel = model.Pesel,
            Address = model.Address,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            Type = "Individual",
            IsDeleted = false
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return new IClientResModel
        {
            Id = client.Id,
            Address = client.Address,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            Type = client.Type,
            Name = client.Name,
            Surname = client.Surname,
            Pesel = client.Pesel
        };
    }

    public async Task DeleteIndividualClientAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client == null)
        {
            throw new NotFoundError($"Client with id {id} was not found");
        }

        if (client.Type != "Individual")
        {
            throw new TypeError("Invalid client type");
        }

        client.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<IClientResModel> UpdateIndividualClientAsync(int id, UpdateIClientReqModel model)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client == null || client.Type != "Individual")
        {
            throw new NotFoundError($"Client with id {id} was not found");
        }

        var individualClient = client as IndividualClient;

        individualClient.Name = model.Name;
        individualClient.Surname = model.Surname;
        individualClient.Address = model.Address;
        individualClient.Email = model.Email;
        individualClient.PhoneNumber = model.PhoneNumber;

        await _context.SaveChangesAsync();

        return new IClientResModel
        {
            Id = individualClient.Id,
            Address = individualClient.Address,
            Email = individualClient.Email,
            PhoneNumber = individualClient.PhoneNumber,
            Type = individualClient.Type,
            Name = individualClient.Name,
            Surname = individualClient.Surname,
            Pesel = individualClient.Pesel
        };
    }

    public async Task<IClientResModel> GetIndividualClientByIdAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client == null || client.Type != "Individual")
        {
            throw new NotFoundError($"Client with id {id} was not found");
        }

        var individualClient = client as IndividualClient;

        return new IClientResModel
        {
            Id = individualClient.Id,
            Address = individualClient.Address,
            Email = individualClient.Email,
            PhoneNumber = individualClient.PhoneNumber,
            Type = individualClient.Type,
            Name = individualClient.Name,
            Surname = individualClient.Surname,
            Pesel = individualClient.Pesel
        };
    }
}