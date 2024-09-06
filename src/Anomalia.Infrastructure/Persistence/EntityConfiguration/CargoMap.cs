using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Persistence.Constants;

namespace Anomalias.Infrastructure.Persistence.EntityConfiguration;
internal class CargoMap : IEntityTypeConfiguration<Cargo>
{
    public void Configure(EntityTypeBuilder<Cargo> builder)
    {
        builder.ToTable(TableNames.Cargos);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever()
               .HasConversion(cargoId => cargoId.Value, value => new(value))
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Ativo)
               .HasColumnType("bit")
               .IsRequired()
               .HasDefaultValue(true);

        builder.Property(c => c.Descricao)
               .HasColumnType("varchar(100)")
               .HasMaxLength(100)
               .IsRequired();

    }
}