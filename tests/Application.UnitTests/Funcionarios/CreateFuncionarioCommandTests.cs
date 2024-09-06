using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Funcionarios.Commands.CreateFuncionario;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Funcionarios;
public class CreateFuncionarioCommandTests
{
   
    private readonly IFuncionarioRepository _funcionarioRepository;
    private readonly CreateFuncionarioCommandHandler _handler;

    public CreateFuncionarioCommandTests()
    {
        _funcionarioRepository = Substitute.For<IFuncionarioRepository>();
        _handler = new CreateFuncionarioCommandHandler(_funcionarioRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCreationFails()
    {
        // Arrange
        var command = new CreateFuncionarioCommand(
            Id: Guid.NewGuid().ToString(),
            Nome: string.Empty, // Forçando falha ao criar
            Email:"email@exemplo.com",
            Setor:SetorId.CreateNew().ToString(),
            Cargo:CargoId.CreateNew().ToString(),
            Gestor:true
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain([DomainErrors.FuncionarioErrors.Create]);
        _funcionarioRepository.DidNotReceive().Add(Arg.Any<Funcionario>());
        await _funcionarioRepository.UnitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCreationSucceeds()
    {
        // Arrange
        var command = new CreateFuncionarioCommand(
          Id: FuncionarioId.CreateNew().ToString(),
          Nome: "teste",
          Email: "teste@exemplo.com",
          Setor: SetorId.CreateNew().ToString(),
          Cargo: CargoId.CreateNew().ToString(),
          Gestor: true
      );


        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(command.Id);
        _funcionarioRepository.Received(1).Add(Arg.Any<Funcionario>());
        await _funcionarioRepository.UnitOfWork.Received(1).CommitAsync(CancellationToken.None);
    }
}
