using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Interfaces;

public interface IDbContext
{

    public DbSet<User> Users { get; set; }
    
    public DbSet<Match> Matches { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<ChatHistory> ChatHistories { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    DbSet<T> Set<T>() where T : class;
}