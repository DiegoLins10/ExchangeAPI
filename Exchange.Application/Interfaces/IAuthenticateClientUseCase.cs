using Exchange.Application.Dtos.Requests;
using Exchange.Application.Dtos.Responses;

namespace Exchange.Application.Interfaces
{
    public interface IAuthenticateClientUseCase
    {
        Task<AuthResponse> ExecuteAsync(AuthRequest request);
    }
}