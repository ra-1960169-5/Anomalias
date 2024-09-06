using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Repository;
using Anomalias.Shared;

namespace Anomalias.Application.Funcionarios.Commands.CreateFuncionario;
internal sealed class CreateFuncionarioCommandHandler(IFuncionarioRepository funcionarioRepository) : ICommandHandler<CreateFuncionarioCommand, string>
{
    private readonly IFuncionarioRepository _funcionarioRepository = funcionarioRepository;


    public async Task<Result<string>> Handle(CreateFuncionarioCommand request, CancellationToken cancellationToken)
    {
        return await
               Domain.Entities.Funcionario.Create(request.Id, request.Nome, request.Email, request.Setor, request.Cargo, request.Gestor)
                      .Tap(_funcionarioRepository.Add)
                      .Map(f => f.Id.ToString())
                      .Tap(() => _funcionarioRepository.UnitOfWork.CommitAsync(cancellationToken));

    }

}
