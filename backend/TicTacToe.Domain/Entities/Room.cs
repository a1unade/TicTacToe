using System.Text.Json.Serialization;
using TicTacToe.Domain.Common;

namespace TicTacToe.Domain.Entities;

public class Room : BaseEntity
{
    public Guid Player1Id { get; set; }
    public User Player1 { get; set; }

    public Guid? Player2Id { get; set; }
    public User? Player2 { get; set; }

    public Guid? MatchId { get; set; }
    
    [JsonIgnore]
    public Match? Match { get; set; }

    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Guid ChatId { get; set; }

    public ChatHistory ChatHistory { get; set; } = default!;
}