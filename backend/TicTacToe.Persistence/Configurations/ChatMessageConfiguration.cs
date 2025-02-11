using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Persistence.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasKey(cm => cm.Id);

        builder.Property(cm => cm.Message)
            .HasMaxLength(500)
            .IsRequired();

        // Настройка связи с User
        builder.HasOne(cm => cm.User)
            .WithMany()
            .HasForeignKey(cm => cm.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Настройка связи с ChatHistory
        builder.HasOne(cm => cm.ChatHistory)
            .WithMany(ch => ch.ChatMessages)
            .HasForeignKey(cm => cm.ChatHistoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}