using TicTacToe.Domain.Common;

namespace TicTacToe.Domain.Entities;

public class User : BaseEntity
{
    public User(string name, string passwordHash)
    {
        Name = name;
        PasswordHash = passwordHash;
    }

    public User()
    {
    }

    public string Name { get; set; }
    public string PasswordHash { get; set; }
    
    public Guid ChatHistoryId { get; set; }
    
    public ChatHistory ChatHistory { get; set; } = default!;
}