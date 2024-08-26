namespace Domain.DTO
{

    public class TransactionDTO
    {
        public string PK { get; set; }
        public decimal amount { get; set; }
        public string clientId { get; set; }
        public string fondoId { get; set; }
    }
}
