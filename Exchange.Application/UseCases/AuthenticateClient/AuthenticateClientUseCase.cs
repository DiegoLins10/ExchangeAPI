using Exchange.Application.Dtos.Requests;
using Exchange.Application.Dtos.Responses;
using Exchange.Application.Interfaces;

namespace Exchange.Application.UseCases.AuthenticateClient
{
    public class AuthenticateClientUseCase : IAuthenticateClientUseCase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateClientUseCase(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public Task<AuthResponse> ExecuteAsync(AuthRequest request)
        {
            var response = _authenticationService.Authenticate(request);

            return response;
        }
    }
}