using Anomalias.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Cargos.Query;
public static class CargoQueryEspecifications
{
    public static IQueryable<Cargo> GetAll(this IQueryable<Cargo> cargos)
    {

        return cargos.AsNoTracking().OrderBy(x => x.Descricao);
    }

    public static IQueryable<Cargo> GetById(this IQueryable<Cargo> cargos, CargoId Id)
    {
        return cargos.AsNoTracking().Where(w => w.Ativo == true && w.Id == Id);

    }
}
