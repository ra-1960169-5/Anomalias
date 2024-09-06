using Anomalia.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Anomalia.Persistence.Constants;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Anomalia.Persistence.EntityConfiguration;
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

     
        builder.HasData(
             Cargo.Create(descricao: "DIRETOR").Value,
             Cargo.Create(descricao: "SUPERVISOR").Value,
             Cargo.Create(descricao: "ANALISTA").Value,
             Cargo.Create(descricao: "ASSISTENTE").Value,
             Cargo.Create(descricao: "AUXILIAR").Value
            );
    }
}