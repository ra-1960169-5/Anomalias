using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Funcionarios.Commands.Update;
using Anomalias.Application.ViewModels;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using NSubstitute;

namespace Application.UnitTests.Funcionarios;
public class UpdateFuncionarioCommandTests
{
    private readonly IFuncionarioRepository _funcionarioRepository;
    private readonly ISetorRepository _setorRepository;
    private readonly UpdateFuncionarioCommandHandler _handler;

    public UpdateFuncionarioCommandTests()
    {
        _funcionarioRepository = Substitute.For<IFuncionarioRepository>();
        _setorRepository = Substitute.For<ISetorRepository>();
        _handler = new UpdateFuncionarioCommandHandler(_funcionarioRepository, _setorRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenFuncionarioDoesNotExist()
    {
        // Arrange
        var command = new UpdateFuncionarioCommand(        
            Id:FuncionarioId.CreateNew(),
            Nome:"Nome",
            SetorId:SetorId.CreateNew().ToString(),
            CargoId:CargoId.CreateNew().ToString(),
            Ativo:true,
            Gestor:false
        );

        _funcionarioRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(null as Funcionario);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.FuncionarioErrors.NotFound);
        _funcionarioRepository.DidNotReceive().Update(Arg.Any<Funcionario>());
        await _funcionarioRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenGestorValidationFails()
    {
        //Arrange
                 
        var funcionarioGestor = Funcionario.Create(FuncionarioId.CreateNew().ToString(), "Nome", "email@email.com", SetorId.CreateNew().ToString(), CargoId.CreateNew().ToString(), true).Value;
        var setor = Setor.Create(
            funcionarioGestor.SetorId,
            "Teste",
            funcionarioGestor
            ).Value;
     
        var funcionario = Funcionario.Create(FuncionarioId.CreateNew().ToString(), "Nome", "email@email.com", setor.Id.ToString(), CargoId.CreateNew().ToString(), true).Value;         

        var command = new UpdateFuncionarioCommand(
              Id: funcionario.Id,
              Nome: "Nome",
              SetorId: funcionario.SetorId.ToString(),
              CargoId: funcionario.CargoId.ToString(),
              Ativo: true,
              Gestor: true
        );

        // Simule métodos ou propriedades específicas
        _funcionarioRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(funcionario);
        _setorRepository.FindByIdWithGestorAsync(SetorId.TryParse(command.SetorId), Arg.Any<CancellationToken>())
            .Returns(setor);
       
    
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();        
              _funcionarioRepository.DidNotReceive().Update(funcionario);
        await _funcionarioRepository.UnitOfWork.DidNotReceive().CommitAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldUpdateFuncionarioAndCommit_WhenValidRequest()
    {
        // Arrange
        var funcionario = Funcionario.Create(FuncionarioId.CreateNew().ToString(),"Nome","email@email.com", SetorId.CreateNew().ToString(), CargoId.CreateNew().ToString(),false).Value;

        var command = new UpdateFuncionarioCommand(
             Id: funcionario.Id,
             Nome: "Nome Atualizado",
             SetorId: SetorId.CreateNew().ToString(),
             CargoId: CargoId.CreateNew().ToString(),
             Ativo: true,
             Gestor: false
         );

        _funcionarioRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(funcionario);
     
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
              _funcionarioRepository.Received(1).Update(funcionario);
        await _funcionarioRepository.UnitOfWork.Received(1).CommitAsync(CancellationToken.None);
    }
}