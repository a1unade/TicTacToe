using TicTacToe.Domain.MongoEntity;

namespace TicTacToe.Application.DTOs;

public class UsersDto
{
    public int Score { get; set; }

    public string Name { get; set; } = default!;
    
    public Guid UserId { get; set; } 
    
    public char Symbol { get; set; }
}