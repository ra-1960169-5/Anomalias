using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Cargos.Commands.DeleteCargo;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Cargos;
public class DeleteCargoCommandTests
{
    private readonly ICargoRepository _cargoRepository;
    private readonly DeleteCargoCommandHandler _handler;

    public DeleteCargoCommandTests()
    {
        _cargoRepository = Substitute.For<ICargoRepository>();
        _handler = new DeleteCargoCommandHandler(_cargoRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCargoNotFound()
    {
        // Arrange
        var command = new DeleteCargoCommand(Id:CargoId.CreateNew());

        // Simulando que o cargo não foi encontrado no repositório
        _cargoRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(null as Cargo);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.CargoErrors.NotFound);
              _cargoRepository.DidNotReceive().Remove(Arg.Any<Cargo>());
        await _cargoRepository.UnitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCargoIsDeletedSuccessfully()
    {
        // Arrange
        var command = new DeleteCargoCommand(Id: CargoId.CreateNew());
        var cargo = Cargo.Create("Test Cargo").Value;

        // Simulando que o cargo foi encontrado no repositório
        _cargoRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(cargo);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
               _cargoRepository.Received(1).Remove(cargo);
        await _cargoRepository.UnitOfWork.Received(1).CommitAsync(CancellationToken.None);
    }
}
