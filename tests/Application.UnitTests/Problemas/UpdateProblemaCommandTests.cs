using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Problemas.Commands.Update;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Problemas;
public class UpdateProblemaCommandHandlerTests
{
    private readonly IProblemaRepository _problemaRepository;
    private readonly UpdateProblemaCommandHandler _handler;

    public UpdateProblemaCommandHandlerTests()
    {
        _problemaRepository = Substitute.For<IProblemaRepository>();
        _handler = new UpdateProblemaCommandHandler(_problemaRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenProblemaIsUpdatedSuccessfully()
    {
        // Arrange
        var command = new UpdateProblemaCommand(Id: ProblemaId.CreateNew(), Descricao: "Descrição Atualizada");

        var problema = Problema.Create("Descrição Original").Value;
        _problemaRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(problema);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        problema.Descricao.Should().Be(command.Descricao.ToUpper());
        _problemaRepository.Received(1).Update(problema);
        await _problemaRepository.UnitOfWork.Received(1).CommitAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProblemaIsNotFound()
    {
        // Arrange
        var command = new UpdateProblemaCommand(Id: ProblemaId.CreateNew(), Descricao: "Descrição Atualizada");

        _problemaRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Problema?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.ProblemaErrors.NotFound);
        _problemaRepository.DidNotReceive().Update(Arg.Any<Problema>());
        await _problemaRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUpdateThrowsException()
    {
        // Arrange
        var command = new UpdateProblemaCommand(Id: ProblemaId.CreateNew(), Descricao: "Descrição Atualizada");

        var problema = Problema.Create("Descrição Original").Value;
        _problemaRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(problema);

        _problemaRepository.When(x => x.Update(problema)).Do(call => { throw new System.Exception("Erro ao atualizar o problema"); });

        // Act
        Func<Task<Result>> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<System.Exception>().WithMessage("Erro ao atualizar o problema");
        await _problemaRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
    }
}
