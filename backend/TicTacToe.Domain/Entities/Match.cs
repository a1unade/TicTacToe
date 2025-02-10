using TicTacToe.Domain.Common;

namespace TicTacToe.Domain.Entities;

public class Match : BaseEntity
{
    public bool IsStarted { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = default!;
    
    public int MaxScore { get; set; }
}