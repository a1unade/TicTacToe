using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Persistence.EfContext;

public class ApplicationDbContext : DbContext, IDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Match> Matches { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Extensions.ServiceCollectionExtensions).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}