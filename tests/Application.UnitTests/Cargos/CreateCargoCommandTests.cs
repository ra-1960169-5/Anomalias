using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Cargos.Commands.CreateCargo;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Cargos;
public class CreateCargoCommandTests
{
    private readonly ICargoRepository _cargoRepository;
    private readonly CreateCargoCommandHandler _handler;
    public CreateCargoCommandTests()
    {
        _cargoRepository = Substitute.For<ICargoRepository>();
        _handler = new CreateCargoCommandHandler(_cargoRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCargoCreationFails()
    {
        // Arrange
        var command = new CreateCargoCommand(Descricao:string.Empty); // Descrição inválida
        // Act
        var result = await _handler.Handle(command, default);
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.CargoErrors.Create);
              _cargoRepository.DidNotReceive().Add(Arg.Any<Cargo>());
        await _cargoRepository.UnitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCargoCreationSucceeds()
    {
        // Arrange
        var command = new CreateCargoCommand(Descricao:"Gerente"); // Descrição válida
       
    // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();      
              _cargoRepository.Received(1).Add(Arg.Any<Cargo>());// Verifica se a anomalia foi adicionada ao repositório
        await _cargoRepository.UnitOfWork.Received(1).CommitAsync(default);//Verifica se o commit foi chamado
    }
}

