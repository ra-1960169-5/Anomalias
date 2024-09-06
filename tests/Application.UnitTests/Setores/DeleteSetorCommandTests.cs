using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Setores.Commands.Delete;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;


namespace Application.UnitTests.Setores;


public class DeleteSetorCommandTests
{
    private readonly ISetorRepository _setorRepository;
    private readonly DeleteSetorCommandHandler _handler;

    public DeleteSetorCommandTests()
    {
        
        _setorRepository = Substitute.For<ISetorRepository>();
        _handler = new DeleteSetorCommandHandler(_setorRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenSetorIsFoundAndDeleted()
    {
        // Arrange
       
        var setor = Setor.Create("Setor Teste").Value;
        var command = new DeleteSetorCommand(setor.Id);

        _setorRepository.GetByIdAsync(setor.Id, Arg.Any<CancellationToken>())
            .Returns(setor);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _setorRepository.Received(1).Remove(setor);
        await _setorRepository.UnitOfWork.Received(1).CommitAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSetorIsNotFound()
    {
        // Arrange
        var setorId = SetorId.CreateNew();
        var command = new DeleteSetorCommand(setorId);

        _setorRepository.GetByIdAsync(setorId, Arg.Any<CancellationToken>())
            .Returns(null as Setor);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.SetorErrors.NotFound);
        _setorRepository.DidNotReceive().Remove(Arg.Any<Setor>());
        await _setorRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
    {
        // Arrange
        var setorId = SetorId.CreateNew();
        var command = new DeleteSetorCommand(setorId);
       
        _setorRepository.GetByIdAsync(setorId, Arg.Any<CancellationToken>()).Throws(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<System.Exception>()
            .WithMessage("Database error");
        _setorRepository.DidNotReceive().Remove(Arg.Any<Setor>());
        await _setorRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
    }
}
