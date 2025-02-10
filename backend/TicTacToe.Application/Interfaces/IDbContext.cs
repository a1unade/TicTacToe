using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Interfaces;

public interface IDbContext
{

    public DbSet<User> Users { get; set; }
    
    public DbSet<Match> Matches { get; set; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    DbSet<T> Set<T>() where T : class;
}