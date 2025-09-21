namespace Exchange.Application.Dtos.Responses
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}