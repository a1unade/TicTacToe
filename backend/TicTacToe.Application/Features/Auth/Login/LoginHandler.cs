using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Responses;

namespace TicTacToe.Application.Features.Auth.Login;

public class LoginHandler : IHandler<LoginCommand, AuthResponse>
{
    private readonly IDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IUserScoreService _scoreService;

    public LoginHandler(IDbContext context, IPasswordHasher passwordHasher, IJwtService jwtService, IUserScoreService scoreService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _scoreService = scoreService;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);

            if (user == null)
            {
                return new AuthResponse
                {
                    IsSuccessfully = false,
                    Message = "User not found."
                };
            }

            var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return new AuthResponse
                {
                    IsSuccessfully = false,
                    Message = "Invalid password."
                };
            }

            var userScore = await _scoreService.GetByUserIdPostgresAsync(user.Id, cancellationToken);

            var token = _jwtService.GenerateToken(user, userScore?.Score ?? 0);

            return new AuthResponse
            {
                IsSuccessfully = true,
                EntityId = user.Id,
                Token = token
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}