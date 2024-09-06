using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anomalias.Infrastructure.Persistence.EntityConfiguration;
internal class SetorMap : IEntityTypeConfiguration<Setor>
{
    public void Configure(EntityTypeBuilder<Setor> builder)
    {
        builder.ToTable(TableNames.Setores);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Ativo)
               .HasColumnType("bit")
               .HasColumnName("Ativo")
               .IsRequired()
               .HasDefaultValue(true);

        builder.Property(c => c.Id).ValueGeneratedNever()
               .HasConversion(setorId => setorId.Value, value => new(value))
               .HasColumnName("Id")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Descricao)
               .HasColumnType("varchar(100)")
               .HasColumnName("Descricao")
               .HasMaxLength(100)
               .IsRequired();


        builder.HasOne(x => x.Gestor).WithOne().HasForeignKey<Funcionario>(x=>x.GestorSetorId);

        builder.HasMany(x => x.Funcionarios).WithOne(x=>x.Setor).HasForeignKey(x=>x.SetorId);

    }



}
