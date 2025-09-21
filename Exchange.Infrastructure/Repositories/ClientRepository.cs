using Exchange.Domain.Entities;
using Exchange.Domain.Interfaces;
using Exchange.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Exchange.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ExchangeDbContext _context;

        public ClientRepository(ExchangeDbContext context)
        {
            _context = context;
        }

        public async Task AddClientAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task<Client?> GetClientByIdAsync(string clientId, string secret)
        {
            var result = await _context.Clients
                .Where(d => d.ClientId == clientId && d.Secret == secret)
                .FirstOrDefaultAsync();

            return result;
        }
    }
}