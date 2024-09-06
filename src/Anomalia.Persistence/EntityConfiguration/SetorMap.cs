using Anomalia.Domain.Entities;
using Anomalia.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anomalia.Persistence.EntityConfiguration;
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

          

        builder.HasMany(x => x.Funcionarios).WithOne(x => x.Setor).HasForeignKey(x => x.SetorId).OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Gestor).WithOne().HasForeignKey<Gestor>(x => x.SetorId);

         builder.HasData(
                 Setor.Create(descricao: "TI").Value,
                 Setor.Create(descricao: "DIRETORIA").Value,
                 Setor.Create(descricao: "COMPRAS").Value,
                 Setor.Create(descricao: "FINANCEIRO").Value,
                 Setor.Create(descricao: "PATRIMÔNIO").Value,
                 Setor.Create(descricao: "MARKETING").Value,
                 Setor.Create(descricao: "RECURSOS HUMANOS").Value,
                 Setor.Create(descricao: "CONTABILIDADE").Value,
                 Setor.Create(descricao: "ALMOXARIFADO").Value,
                 Setor.Create(descricao: "QUALIDADE").Value,
                 Setor.Create(descricao: "ESTOQUE").Value,
                 Setor.Create(descricao: "DISTRIBUIÇÃO").Value
            );
    }

    

}
