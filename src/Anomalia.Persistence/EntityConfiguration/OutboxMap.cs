using Anomalia.Persistence.Constants;
using Anomalia.Infrastructure.OutBox;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Anomalia.Persistence.EntityConfiguration;
internal sealed class OutboxMessageMap : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(TableNames.OutboxMessages);

        builder.HasKey(x => x.Id);
    }
}