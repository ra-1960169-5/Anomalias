

using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Problemas.Commands.Delete;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace Application.UnitTests.Problemas;
public class DeleteProblemaCommandTests
{
    private readonly IProblemaRepository _problemaRepository;
    private readonly DeleteProblemaCommandHandler _handler;

    public DeleteProblemaCommandTests()
    {
        _problemaRepository = Substitute.For<IProblemaRepository>();
        _handler = new DeleteProblemaCommandHandler(_problemaRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProblemaIsDeletedSuccessfully()
    {
        // Arrange
        var command = new DeleteProblemaCommand(ProblemaId.CreateNew());
        var problema = Problema.Create("Descrição do Problema").Value;

        // Simulação de retorno do repositório
        _problemaRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(problema);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue(); // Verifica se o resultado foi sucesso
        _problemaRepository.Received(1).Remove(problema); // Verifica se o método Remove foi chamado uma vez
        await _problemaRepository.UnitOfWork.Received(1).CommitAsync(CancellationToken.None); // Verifica se Commit foi chamado uma vez
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProblemaNotFound()
    {
        // Arrange
        var command = new DeleteProblemaCommand(ProblemaId.CreateNew());

        // Simulação de retorno nulo do repositório
        _problemaRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(null as Problema);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue(); // Verifica se o resultado foi falha
        result.Errors.Should().Contain(DomainErrors.ProblemaErrors.NotFound); // Verifica se o erro correto foi retornado
        _problemaRepository.DidNotReceive().Remove(Arg.Any<Problema>()); // Verifica se o método Remove não foi chamado
        await _problemaRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None); // Verifica se Commit não foi chamado
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
    {
        // Arrange
        var command = new DeleteProblemaCommand(ProblemaId.CreateNew());
        var problema = Problema.Create("Descrição do Problema").Value;

        // Simulação de retorno do repositório
        _problemaRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(problema);

        // Simulação de uma exceção ao tentar remover o problema do repositório
        _problemaRepository.When(x => x.Remove(problema)).Do(call => { throw new Exception("Erro ao acessar o banco de dados"); });

        // Act
        Func<Task<Result>> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Erro ao acessar o banco de dados"); // Verifica se a exceção correta foi lançada
             _problemaRepository.Received(1).Remove(Arg.Any<Problema>()); // Verifica se o método Remove não foi chamado
        await _problemaRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None); // Verifica se Commit não foi chamado
    }
}
