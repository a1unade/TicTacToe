using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        // Настройка связи с Player1
        builder.HasOne(r => r.Player1)
            .WithMany()
            .HasForeignKey(r => r.Player1Id)
            .OnDelete(DeleteBehavior.Restrict);

        // Настройка связи с Player2
        builder.HasOne(r => r.Player2)
            .WithMany()
            .HasForeignKey(r => r.Player2Id)
            .OnDelete(DeleteBehavior.Restrict);

        // Настройка связи с ChatHistory
        builder.HasOne(r => r.ChatHistory)
            .WithOne(ch => ch.Room)
            .HasForeignKey<ChatHistory>(ch => ch.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(r => r.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .IsRequired();
    }
}