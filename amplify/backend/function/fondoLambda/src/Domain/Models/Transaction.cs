

namespace Domain.Models
{
  public class Transaction
  {
    public string PK { get; set; }
    public string SK { get; set; }
    public string id { get; set; }
    public decimal amount { get; set; }
    public string clientId {get; set;}
  }
}
