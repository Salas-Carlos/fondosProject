using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Infrastructure.Repositories;

namespace Application.Services
{
  public class FondoService
  {
    private readonly DynamoDbRepository<Fondo> _repository;

    public FondoService(DynamoDbRepository<Fondo> repository)
    {
        _repository = repository;
    }

    public Task<Fondo> GetFondoAsync(string id) => _repository.GetAsync("FONDOS", $"id#{id}");
    public Task<IEnumerable<Fondo>> GetAllFondosAsync() => _repository.GetAllAsync("FONDOS", "id#");
    public Task AddFondoAsync(Fondo fondo) => _repository.AddAsync(fondo);
    public Task UpdateFondoAsync(Fondo fondo) => _repository.UpdateAsync(fondo);
    public Task DeleteFondoAsync(string pk, string sk) => _repository.DeleteAsync(pk, sk);
  }

}
