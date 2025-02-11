using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Persistence.Configurations;

public class ChatHistoryConfiguration : IEntityTypeConfiguration<ChatHistory>
{
    public void Configure(EntityTypeBuilder<ChatHistory> builder)
    {
        builder.HasKey(ch => ch.Id);

        builder.Property(ch => ch.StartDate)
            .HasColumnType("date")
            .IsRequired();

        // Настройка связи с Room
        builder.HasOne(ch => ch.Room)
            .WithOne(r => r.ChatHistory)
            .HasForeignKey<ChatHistory>(ch => ch.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        // Настройка связи с User
        builder.HasOne(ch => ch.User)
            .WithOne(u => u.ChatHistory)
            .HasForeignKey<ChatHistory>(ch => ch.UserId) // Указываем, что ChatHistory зависит от User
            .OnDelete(DeleteBehavior.Cascade);

        // Настройка связи с ChatMessages
        builder.HasMany(ch => ch.ChatMessages)
            .WithOne(cm => cm.ChatHistory)
            .HasForeignKey(cm => cm.ChatHistoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}