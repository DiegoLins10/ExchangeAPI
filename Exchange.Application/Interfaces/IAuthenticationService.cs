using Exchange.Application.Dtos.Requests;
using Exchange.Application.Dtos.Responses;

namespace Exchange.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResponse> Authenticate(AuthRequest request);
    }
}