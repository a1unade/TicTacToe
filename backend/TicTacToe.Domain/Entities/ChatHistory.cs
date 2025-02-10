using TicTacToe.Domain.Common;

namespace TicTacToe.Domain.Entities;

public class ChatHistory : BaseEntity
{
    public DateOnly StartDate { get; set; }
    
    public Guid UserId { get; set; }

    public User User { get; set; } = default!;

    public ICollection<ChatMessage> ChatMessages { get; set; } = default!;
}