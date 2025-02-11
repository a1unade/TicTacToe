using System.Text.Json.Serialization;
using TicTacToe.Domain.Common;

namespace TicTacToe.Domain.Entities;

public class Match : BaseEntity
{
    public bool IsStarted { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = "Waiting";
    public int MaxScore { get; set; }

    public Guid RoomId { get; set; } // Внешний ключ для Room
    [JsonIgnore]
    public Room Room { get; set; } // Навигационное свойство

    public string Board { get; set; }
    public Guid CurrentPlayerId { get; set; }
    public Guid? WinnerId { get; set; }
}
