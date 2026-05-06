using Inventario.Application.Common.Interfaces;
using Inventario.Application.Common.Models;
using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Commands.Auth.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<Result<AuthResponse>>;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        private readonly IAuthUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenProvider _tokenProvider;

        public LoginCommandHandler(
            IAuthUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenProvider tokenProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenProvider = tokenProvider;
        }

        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);

            if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return Result<AuthResponse>.Failure("Credenciales inválidas.");
            }

            if (!user.IsActive)
            {
                return Result<AuthResponse>.Failure("Usuario inactivo.");
            }

            var token = _tokenProvider.Generate(user);

            return Result<AuthResponse>.Success(new AuthResponse(
                user.Id,
                user.Username,
                token,
                user.Role
            ));
        }
    }
}
