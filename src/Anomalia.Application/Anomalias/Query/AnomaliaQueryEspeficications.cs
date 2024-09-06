using Anomalias.Domain.Entities;
using Anomalias.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Anomalias.Application.Anomalias.Query;

public static class AnomaliaQueryEspeficications
{
    public static IQueryable<Anomalia> GetById(this IQueryable<Anomalia> anomalias, AnomaliaId id)
    {
        return anomalias.Where(x => x.Id == id);
    }

    public static IQueryable<Anomalia> GetAll(this IQueryable<Anomalia> anomalias, DateTime dataInicial, DateTime dataFinal, int status, SetorId setorId)
    {
        return anomalias
              .AsNoTracking()
              .Include(x => x.FuncionarioAbertura)
              .Include(x => x.FuncionarioEncerramento)
              .Include(x => x.Problema)
              .Include(x => x.Setor)
              .Where(x => x.DataDeAbertura.Date >= dataInicial
                                 && x.DataDeAbertura.Date <= dataFinal
                                 && !x.Restrita
                                 && x.SetorId != setorId
                                 && x.Status.Equals((EStatus)status)).OrderBy(x => x.Numero);
    }

    public static IQueryable<Anomalia> GetByUserId(this IQueryable<Anomalia> anomalias, DateTime dataInicial, DateTime dataFinal, int status, FuncionarioId userId)
    {
        return anomalias.AsNoTracking()
              .Include(x => x.FuncionarioAbertura)
              .Include(x => x.FuncionarioEncerramento)
              .Include(x => x.Problema)
              .Include(x => x.Setor)
              .Where(x => x.DataDeAbertura.Date >= dataInicial
                                 && x.DataDeAbertura.Date <= dataFinal
                                 && x.AberturaId.Equals(userId)
                                 && x.Status.Equals((EStatus)status)).OrderBy(x => x.Numero);
    }

    public static IQueryable<Anomalia> GetBySetorId(this IQueryable<Anomalia> anomalias, DateTime dataInicial, DateTime dataFinal, int status, SetorId setorId)
    {
        return anomalias.AsNoTracking()
              .Include(x => x.FuncionarioAbertura)
              .Include(x => x.FuncionarioEncerramento)
              .Include(x => x.Problema)
              .Include(x => x.Setor)
              .Where(x => x.DataDeAbertura.Date >= dataInicial
                                 && x.DataDeAbertura.Date <= dataFinal
                                 && x.SetorId.Equals(setorId)
                                 && x.Status.Equals((EStatus)status)).OrderBy(x => x.Numero);
    }


}