using System.Text.Json.Serialization;

namespace Exchange.Application.Dtos.Requests
{
    public class AuthRequest
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}