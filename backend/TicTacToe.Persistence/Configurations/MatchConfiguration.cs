using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Persistence.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasKey(m => m.Id);

        // Настройка связи с Room
        builder.HasOne(m => m.Room)
            .WithOne(r => r.Match)
            .HasForeignKey<Match>(m => m.RoomId) // Внешний ключ в Match
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.Board)
            .HasMaxLength(9)
            .IsRequired();
    }
}