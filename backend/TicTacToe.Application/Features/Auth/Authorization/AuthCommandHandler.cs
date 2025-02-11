using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Responses;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.MongoEntity;

namespace TicTacToe.Application.Features.Auth.Authorization;

public class AuthCommandHandler : IHandler<AuthCommand, AuthResponse>
{
    private readonly IDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly IUserScoreService _userScoreService;

    public AuthCommandHandler(IDbContext context, IPasswordHasher passwordHasher, IJwtService jwtService,
        IUserScoreService userScoreService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _userScoreService = userScoreService;
    }

    public async Task<AuthResponse> Handle(AuthCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);

        if (existingUser != null)
        {
            return new AuthResponse
            {
                IsSuccessfully = false,
                Message = "User with this name already exists."
            };
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = new User(request.Name, passwordHash);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var userScore = new UserScore
        {
            Id = ObjectId.GenerateNewId(),
            Name = request.Name,
            Score = 0,
            UserIdPostgres = user.Id
        };

        await _userScoreService.CreateAsync(userScore, cancellationToken);

        var token = _jwtService.GenerateToken(user, userScore.Score);

        return new AuthResponse
        {
            IsSuccessfully = true,
            EntityId = user.Id,
            Token = token
        };
    }
}