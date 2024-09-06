using Microsoft.EntityFrameworkCore;
using Anomalias.Domain.Entities;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Infrastructure.Persistence.Data;

namespace Anomalias.Infrastructure.Persistence.Repository;
public class FuncionarioRepository(ApplicationDbContext context) : Repository<Funcionario, FuncionarioId>(context), IFuncionarioRepository
{  
}
