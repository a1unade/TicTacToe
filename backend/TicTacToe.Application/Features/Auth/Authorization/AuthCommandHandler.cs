using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.Interfaces;
using TicTacToe.Application.Responses;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Features.Auth.Authorization;

public class AuthCommandHandler : IHandler<AuthCommand, AuthResponse>
{
    private readonly IDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthCommandHandler(IDbContext context, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(AuthCommand request, CancellationToken cancellationToken)
    {
        try
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

            var token = _jwtService.GenerateToken(user);

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

