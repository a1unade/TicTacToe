using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Name).IsUnique(); 

        builder.Property(u => u.Name)
            .HasMaxLength(50) 
            .IsRequired(); 

        builder.Property(u => u.PasswordHash)
            .IsRequired(); 

        builder.Property(u => u.Score)
            .HasDefaultValue(0); 
    }
}