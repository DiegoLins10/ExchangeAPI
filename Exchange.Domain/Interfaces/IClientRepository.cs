using Exchange.Domain.Entities;

namespace Exchange.Domain.Interfaces
{
    public interface IClientRepository
    {
        Task AddClientAsync(Client client);
        Task<Client?> GetClientByIdAsync(string clientId, string secret);
    }
}