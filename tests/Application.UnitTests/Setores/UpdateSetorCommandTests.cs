namespace Application.UnitTests.Setores;

using FluentAssertions;
using global::Anomalias.Application.Abstractions.Repository;
using global::Anomalias.Application.Setores.Commands.Update;
using global::Anomalias.Domain.Entities;
using global::Anomalias.Domain.Errors;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class UpdateSetorCommandHandlerTests
{
    private readonly ISetorRepository _setorRepository;
    private readonly IFuncionarioRepository _funcionarioRepository;
    private readonly UpdateSetorCommandHandler _handler;

    public UpdateSetorCommandHandlerTests()
    {
        // Arrange - Setup global mocks and dependencies
        _setorRepository = Substitute.For<ISetorRepository>();
        _funcionarioRepository = Substitute.For<IFuncionarioRepository>();
        _handler = new UpdateSetorCommandHandler(_setorRepository, _funcionarioRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenSetorIsFoundAndUpdated()
    {
        // Arrange
        var setorId = SetorId.CreateNew();
        var gestorId = FuncionarioId.CreateNew();
        var setor = Setor.Create("Setor Teste").Value;
        var gestor = Funcionario.Create(gestorId.ToString(), "Nome", "email@email.com", setorId.ToString(), CargoId.CreateNew().ToString(), true).Value;

        var command = new UpdateSetorCommand(
            Id:setorId,
            Descricao:"Nova Descrição",
            GestorId: gestorId.ToString()
        );

        _setorRepository.FindByIdWithGestorAsync(setorId, Arg.Any<CancellationToken>())
            .Returns(setor);
        _funcionarioRepository.GetByIdAsync(gestorId, Arg.Any<CancellationToken>())
            .Returns(gestor);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _setorRepository.Received(1).Update(Arg.Is<Setor>(s => s.Descricao == "NOVA DESCRIÇÃO" && s.Gestor == gestor));
        await _setorRepository.UnitOfWork.Received(1).CommitAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSetorIsNotFound()
    {
        // Arrange
        var setorId = SetorId.CreateNew();
        var command = new UpdateSetorCommand
            (Id: setorId,
            Descricao: "Nova Descrição",
            GestorId:FuncionarioId.CreateNew().ToString()
             );

        _setorRepository.FindByIdWithGestorAsync(setorId, Arg.Any<CancellationToken>())
            .Returns(null as Setor);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.SetorErrors.NotFound);
        _setorRepository.DidNotReceive().Update(Arg.Any<Setor>());
        await _setorRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
    }   

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSetorUpdateFails()
    {
        // Arrange
        var setorId = SetorId.CreateNew();
        var gestorId = FuncionarioId.CreateNew();
        var setor = Setor.Create("Setor Teste").Value;
        var gestor = Funcionario.Create(gestorId.ToString(), "Nome", "email@email.com", setorId.ToString(), CargoId.CreateNew().ToString(), true).Value;

        var command = new UpdateSetorCommand
        (
            Id :setorId,
            Descricao: string.Empty, // Forçar falha na atualização
            GestorId:gestorId.ToString()
        );

        _setorRepository.FindByIdWithGestorAsync(setorId, Arg.Any<CancellationToken>())
            .Returns(setor);

        _funcionarioRepository.GetByIdAsync(gestorId, Arg.Any<CancellationToken>())
            .Returns(gestor);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.SetorErrors.Update);
        _setorRepository.DidNotReceive().Update(Arg.Any<Setor>());
        await _setorRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
    {
        // Arrange
        var setorId = SetorId.CreateNew();
        var gestorId = FuncionarioId.CreateNew();
        var command = new UpdateSetorCommand
        (
            Id:setorId,
            Descricao: "Nova Descrição",
            GestorId : gestorId.ToString()
        );

        _setorRepository.FindByIdWithGestorAsync(setorId, Arg.Any<CancellationToken>()).Throws(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<System.Exception>()
            .WithMessage("Database error");

        _setorRepository.DidNotReceive().Update(Arg.Any<Setor>());
        await _setorRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
    }

   

}
