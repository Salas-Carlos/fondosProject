using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Infrastructure.Repositories;

namespace Application.Services
{
  public class TransactionService
  {
    private readonly DynamoDbRepository<Transaction> _repository;
    private readonly ClientService _clientService;

    public TransactionService(DynamoDbRepository<Transaction> repository, ClientService clientService)
    {
        _repository = repository;
        _clientService = clientService;
    }

    public Task<Transaction> GetTransactionAsync(string id) => _repository.GetAsync("TRANSACTION", $"id#{id}");
    public Task<IEnumerable<Transaction>> GetAllTransactionsAsync() => _repository.GetAllAsync("TRANSACTION", "id#");

    public async Task AddTransactionAsync(Transaction transaction)
    {
        var client = await _clientService.GetClientAsync(transaction.clientId);
        if (client != null)
        {
            client.amountClient += transaction.amount;
            await _clientService.UpdateClientAsync(client);
        }

        await _repository.AddAsync(transaction);
    }

    public Task UpdateTransactionAsync(Transaction transaction) => _repository.UpdateAsync(transaction);

    public async Task DeleteTransactionAsync(string pk, string sk)
    {
        var transaction = await _repository.GetAsync(pk, sk);
        if (transaction != null)
        {
            var client = await _clientService.GetClientAsync(transaction.clientId);
            if (client != null)
            {
                client.amountClient -= transaction.amount;
                await _clientService.UpdateClientAsync(client);
            }
        }

        await _repository.DeleteAsync(pk, sk);
    }
  }

}
