using Domain.DTO;

namespace Domain.Models
{
  public class Transaction
  {
    public string PK { get; set; }
    public string SK { get; set; }
    public string id { get; set; }
    public decimal amount { get; set; }
    public string clientId {get; set;}
    public string fondoId {get; set;}

    public Transaction(TransactionDTO dto)
    {
        PK = dto.PK;
        amount = dto.amount;
        clientId = dto.clientId;
        fondoId = dto.fondoId;
    }
    public Transaction() { }

    public override string ToString()
    {
        return $"PK: {PK}, SK: {SK}, id: {id}, amount: {amount}, clientId: {clientId}, fondoId: {fondoId}";
    }
  }
}
