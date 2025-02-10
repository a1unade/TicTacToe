using TicTacToe.Domain.Common;

namespace TicTacToe.Domain.Entities;

public class ChatMessage : BaseEntity
{
    public string Message { get; set; } = default!;

    public TimeOnly Time { get; set; }

    public DateOnly Date { get; set; }

    public Guid UserId { get; set; }
    
    public User User { get; set; } = default!;
    
    public Guid ChatHistoryId { get; set; }

    public ChatHistory ChatHistory { get; set; } = default!;
}