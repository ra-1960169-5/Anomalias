using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Problemas.Commands.Create;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Problemas;
public class CreateProblemaCommandTests
{
    private readonly IProblemaRepository _problemaRepository;
    private readonly CreateProblemaCommandHandler _handler;

    public CreateProblemaCommandTests()
    {
        // Arrange - Setup global mocks and dependencies
        _problemaRepository = Substitute.For<IProblemaRepository>();
        _handler = new CreateProblemaCommandHandler(_problemaRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProblemaIsCreatedSuccessfully()
    {
        // Arrange
        var command = new CreateProblemaCommad(Descricao:"Descrição do Problema");

        var problema = Problema.Create(command.Descricao.ToUpper()).Value;

        // Simulação do método Create de Problema e adição ao repositório       
        _problemaRepository.Add(problema);
        _problemaRepository.UnitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
              _problemaRepository.Received(1).Add(problema);
        await _problemaRepository.UnitOfWork.Received(1).CommitAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProblemaCreationFails()
    {
        // Arrange
        var command = new CreateProblemaCommad(Descricao: string.Empty); // Descrição vazia para simular falha
          
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.ProblemaErrors.Create);
        _problemaRepository.DidNotReceive().Add(Arg.Any<Problema>());
        await _problemaRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
    {
        // Arrange
        var command = new CreateProblemaCommad(Descricao: "Descrição do Problema");
            
        // Simulação de uma exceção ao tentar remover o problema do repositório
        _problemaRepository.When(x => x.Add(Arg.Any<Problema>())).Do(call => { throw new Exception("Erro ao acessar o banco de dados"); });

        // Act
        Func<Task<Result>> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Erro ao acessar o banco de dados"); // Verifica se a exceção correta foi lançada
              _problemaRepository.Received(1).Add(Arg.Any<Problema>()); // Verifica se o método Remove não foi chamado
        await _problemaRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None); // Verifica se Commit não foi chamado
    }

}
