using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Outbox;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type).IsRequired();

        builder.Property(x => x.Content).IsRequired();

        builder.Property(x => x.OccurredOnUtc).IsRequired();
    }
}
