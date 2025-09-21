namespace Exchange.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; } // PK
        public string ClientId { get; set; }
        public string Secret { get; set; }
    }
}