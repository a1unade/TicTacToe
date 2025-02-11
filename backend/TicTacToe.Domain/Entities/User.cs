using System.Text.Json.Serialization;
using TicTacToe.Domain.Common;

namespace TicTacToe.Domain.Entities;

public class User : BaseEntity
{
    public User(string name, string passwordHash)
    {
        Name = name;
        PasswordHash = passwordHash;
        Score = 0;
    }

    public User()
    {
    }

    public string Name { get; set; }
    public int Score { get; set; }
    public string PasswordHash { get; set; }
    public Guid ChatHistoryId { get; set; }
    
    public ChatHistory ChatHistory { get; set; } = default!;
}