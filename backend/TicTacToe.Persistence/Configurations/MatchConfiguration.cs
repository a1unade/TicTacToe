using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Persistence.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status);

        builder.Property(x => x.IsStarted);

        builder.Property(x => x.MaxScore);

        builder.Property(x => x.CreatedAt);
    }
}