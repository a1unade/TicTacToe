using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Persistence.Configurations;

public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Time)
            .IsRequired();
        
        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.Message);


        builder.HasOne(x => x.ChatHistory)
            .WithMany(x => x.ChatMessages)
            .HasForeignKey(x => x.ChatHistoryId);
    }
}