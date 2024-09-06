using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Anomalias.Infrastructure.Persistence.EntityConfiguration;
internal class ProblemaMap : IEntityTypeConfiguration<Problema>
{
    public void Configure(EntityTypeBuilder<Problema> builder)
    {
        builder.ToTable(TableNames.Problemas);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Ativo)
               .HasColumnType("bit")
               .HasColumnName("Ativo")
               .IsRequired()
               .HasDefaultValue(true);

        builder.Property(c => c.Id).ValueGeneratedNever()
               .HasConversion(problemaId => problemaId.Value, value => new(value))
               .HasColumnName("Id")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Descricao)
               .HasColumnName("Descricao")
               .HasColumnType("varchar(100)")
               .HasMaxLength(100)
               .IsRequired();

        builder.HasMany(x => x.Anomalias).WithOne(x => x.Problema);

    }
}
