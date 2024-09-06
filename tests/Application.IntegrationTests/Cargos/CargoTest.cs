using Anomalias.Application.Cargos.Commands.CreateCargo;
using Anomalias.Application.Cargos.Commands.DeleteCargo;
using Anomalias.Application.Cargos.Commands.UpdateCargo;
using Anomalias.Application.Cargos.Query.GetById;
using Anomalias.Domain.Entities;
using Application.IntegrationTests.Fixtures;
using FluentAssertions;

namespace Application.IntegrationTests.Cargos;
public class CargoTest(CustomWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact(DisplayName = "Deve retornar sucesso quando a criação de um cargo for bem-sucedida")]
    public async Task Handle_ShouldReturnSuccess_WhenCargoCreationSucceeds()
    {
        CreateCargoCommand createCargoCommand = new("Teste de Criação");
        var resultCreateCargoCommand = await Mediator.SendCommandAsync(createCargoCommand);
        resultCreateCargoCommand.IsSuccess.Should().BeTrue();
        CargoId cargoId = CargoId.TryParse(resultCreateCargoCommand.Value)!.Value;
        var resultGetCargo = await Mediator.SendCommandAsync(new GetByIdCargoQuery(cargoId));
            resultGetCargo.IsSuccess.Should().BeTrue();
            resultGetCargo.Value.Descricao.Should().Be(createCargoCommand.Descricao);
            resultGetCargo.Value.Id.Should().Be(cargoId.ToString());
    }



    [Fact(DisplayName = "Deve retornar sucesso quando a deleção de um cargo for bem-sucedida")]
    public async Task Handle_ShouldReturnSuccess_WhenCargoDeletionSucceeds()
    {
        CreateCargoCommand createCargoCommand = new("Teste de Exclusão");
        var resultCreateCargoCommand = await Mediator.SendCommandAsync(createCargoCommand);     
        CargoId cargoId = CargoId.TryParse(resultCreateCargoCommand.Value)!.Value;
        DeleteCargoCommand deleteCargoCommand = new(cargoId);
        var resultDeleteCargoCommand = await Mediator.SendCommandAsync(deleteCargoCommand);
            resultDeleteCargoCommand.IsSuccess.Should().BeTrue();
    }


    [Fact(DisplayName = "Deve retornar sucesso quando a atualização de um cargo for bem-sucedida")]
    public async Task Handle_ShouldReturnSuccess_WhenCargoUpdateSucceeds()
    {
        CreateCargoCommand createCargoCommand = new("Teste de atualização");
        var resultCreateCargoCommand = await Mediator.SendCommandAsync(createCargoCommand);
        CargoId cargoId = CargoId.TryParse(resultCreateCargoCommand.Value)!.Value;
        UpdateCargoCommand updateCargoCommand = new(cargoId, "cargo atualizado");
        var resultUpdateCargoCommand = await Mediator.SendCommandAsync(updateCargoCommand);
            resultUpdateCargoCommand.IsSuccess.Should().BeTrue();
        var resultGetCargo = await Mediator.SendCommandAsync(new GetByIdCargoQuery(cargoId));
        resultGetCargo.IsSuccess.Should().BeTrue();
        resultGetCargo.Value.Descricao.Should().Be(updateCargoCommand.Descricao);
        resultGetCargo.Value.Id.Should().Be(cargoId.ToString());
    }



}
