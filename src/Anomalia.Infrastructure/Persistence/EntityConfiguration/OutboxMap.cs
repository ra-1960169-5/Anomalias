using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Anomalias.Infrastructure.Persistence.Constants;
using Anomalias.Infrastructure.OutBox;

namespace Anomalias.Infrastructure.Persistence.EntityConfiguration;
internal sealed class OutboxMessageMap : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(TableNames.OutboxMessages);

        builder.HasKey(x => x.Id);
    }
}