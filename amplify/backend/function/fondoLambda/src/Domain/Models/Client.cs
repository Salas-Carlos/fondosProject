using System.Collections.Generic;


namespace Domain.Models
{
  public class Client
  {
    public string PK { get; set; }
    public string SK { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public decimal amountClient { get; set; }
    public List<string> Funds { get; set; }

  }
}
