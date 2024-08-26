using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Infrastructure.Repositories;

namespace Application.Services
{
   public class ClientService
{
    private readonly DynamoDbRepository<Client> _repository;

    public ClientService(DynamoDbRepository<Client> repository)
    {
        _repository = repository;
    }

    public Task<Client> GetClientAsync(string id) => _repository.GetAsync("CLIENT", $"id#{id}");
    public Task<IEnumerable<Client>> GetAllClientsAsync() => _repository.GetAllAsync("CLIENT", "id#");
    public Task AddClientAsync(Client client) => _repository.AddAsync(client);
    public Task UpdateClientAsync(Client client) => _repository.UpdateAsync(client);
    public Task DeleteClientAsync(string pk, string sk) => _repository.DeleteAsync(pk, sk);
}
}
