using Anomalia.Domain.Entities;
using Anomalia.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Anomalia.Persistence.EntityConfiguration;
internal class AnexoMap : IEntityTypeConfiguration<Anexo>
{
    public void Configure(EntityTypeBuilder<Anexo> builder)
    {
        builder.ToTable(TableNames.Anexos);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .ValueGeneratedNever()
               .HasConversion(anexo=>anexo.Value,value => new(value))              
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Nome)
               .HasColumnType("varchar(200)")
               .HasColumnName("Nome")
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(c => c.ContentType)
               .HasColumnType("varchar(100)")
               .HasColumnName("ContentType")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Path)
               .HasColumnType("varchar(200)")
               .HasColumnName("Path")
               .HasMaxLength(200)
               .IsRequired();


    }
}
