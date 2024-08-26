using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.DTO;
using Infrastructure.Repositories;
using System;
using System.Linq;


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

    public async Task<Transaction> AddTransactionAsync(TransactionDTO transactionDTO)
    {
        var client = await _clientService.GetClientAsync(transactionDTO.clientId);
        if (client != null)
        {
            client.amountClient += transactionDTO.amount;
            if(transactionDTO.amount<0){
              client.funds.Add(transactionDTO.fondoId);
            }else{
             IEnumerable<string> filteredFunds = client.funds.Where(fund=> fund != transactionDTO.fondoId);
             client.funds = filteredFunds.ToList();
            }

            await _clientService.UpdateClientAsync(client);
        }
        Console.WriteLine("transactionDTO: ", transactionDTO.ToString());
        var transaction = new Transaction(transactionDTO);
        var id = Guid.NewGuid().ToString();
        transaction.id =id;
        transaction.SK = $"id#{id}";
        Console.WriteLine("transaction: ", transaction.ToString());
        return await _repository.AddAsync(transaction);

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
