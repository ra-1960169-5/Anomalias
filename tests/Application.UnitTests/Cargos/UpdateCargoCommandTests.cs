using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Cargos.Commands.UpdateCargo;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Cargos;
public class UpdateCargoCommandTests
{
    private readonly ICargoRepository _cargoRepository;
    private readonly UpdateCargoCommandHandler _handler;

    public UpdateCargoCommandTests()
    {
        _cargoRepository = Substitute.For<ICargoRepository>();
        _handler = new UpdateCargoCommandHandler(_cargoRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCargoDoesNotExist()
    {
        // Arrange
        var command = new UpdateCargoCommand(Id:CargoId.CreateNew(), Descricao:"Nova Descrição");
        _cargoRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(null as Cargo);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.CargoErrors.NotFound);
              _cargoRepository.DidNotReceive().Update(Arg.Any<Cargo>());
        await _cargoRepository.UnitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldUpdateCargo_WhenCargoExists()
    {
        // Arrange
        var command = new UpdateCargoCommand(Id: CargoId.CreateNew(), Descricao: "Nova Descrição");        
        var cargo = Cargo.Restore(command.Id,command.Descricao).Value;

        _cargoRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(cargo);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();      
              _cargoRepository.Received(1).Update(cargo);
        await _cargoRepository.UnitOfWork.Received(1).CommitAsync(CancellationToken.None);
    }
}
