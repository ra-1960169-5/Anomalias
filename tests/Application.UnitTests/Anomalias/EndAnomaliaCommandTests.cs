using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Anomalias.Commands.EndAnomalia;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Anomalias;
public class EndAnomaliaCommandTests
{
    private readonly IAnomaliaRepository _anomaliaRepository;
    private readonly EndAnomaliaCommandHandler _handler;
    public EndAnomaliaCommandTests()
    {
        _anomaliaRepository = Substitute.For<IAnomaliaRepository>();
        _handler = new EndAnomaliaCommandHandler(_anomaliaRepository);
    }

    [Fact]
    public async Task Handle_EndAnomaliaSuccessfully()
    {
        // Arrange
        var anomaliaId = AnomaliaId.CreateNew();
        var anomalia = Anomalia.Create(          
            problemaId:ProblemaId.CreateNew().ToString(),
            setorId:SetorId.CreateNew().ToString(),
            restrita:false,
            dataDeAbertura:DateTime.Now,
            usuarioAberturaId:FuncionarioId.CreateNew().Value,
            questionamento:"Questionamento da anomalia",
            resultadoEsperado:"Resultado esperado da anomalia"
            ).Value; //cria uma instancia válidade de anomalia

        _anomaliaRepository.GetByIdAsync(anomaliaId, default).Returns(anomalia);
        _anomaliaRepository.When(a => a.Update(anomalia)).Do(call => { });
        _anomaliaRepository.UnitOfWork.When(u => u.CommitAsync(default)).Do(call => { });

        // Act
        var result = await _handler.Handle(new EndAnomaliaCommand(anomaliaId.ToString(),"considerações finais",FuncionarioId.CreateNew().Value,DateTime.Now), default);

        // Assert
        result.IsSuccess.Should().BeTrue();  //Verifica se o resultado foi um sucesso       
        _anomaliaRepository.Received(1).Update(Arg.Any<Anomalia>()); // Verifica se a anomalia foi atualizada 
        await _anomaliaRepository.UnitOfWork.Received(1).CommitAsync(default); //Verifica se o commit foi chamado

    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAnomaliaIsNotFound()
    {
        // Arrange
        var command = new EndAnomaliaCommand(Guid.Empty.ToString(), "considerações finais", FuncionarioId.CreateNew().Value, DateTime.Now);

        _anomaliaRepository.GetByIdAsync(Arg.Any<AnomaliaId>(), Arg.Any<CancellationToken>())
            .Returns(null as Anomalia);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.AnomaliaErrors.NotFound);
        await _anomaliaRepository.UnitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>()); // Verifica se o commit não foi chamado

    }

    [Fact]
    public async Task Handle_EndAnomaliaFailsInDomain()
    {
        // Arrange
       
        var anomalia = Anomalia.Create(
            problemaId: ProblemaId.CreateNew().ToString(),
            setorId: SetorId.CreateNew().ToString(),
            restrita: false,
            dataDeAbertura: DateTime.Now,
            usuarioAberturaId: FuncionarioId.CreateNew().Value,
            questionamento: "Questionamento da anomalia",
            resultadoEsperado: "Resultado esperado da anomalia"
            ).Value;  //cria uma instancia válidade de anomalia

        var command = new EndAnomaliaCommand(anomalia.Id.ToString(), "", FuncionarioId.CreateNew().Value, DateTime.Now);

        _anomaliaRepository.GetByIdAsync(anomalia.Id, default).Returns(anomalia);
        _anomaliaRepository.When(a => a.Update(anomalia)).Do(call => { });
        _anomaliaRepository.UnitOfWork.When(u => u.CommitAsync(default)).Do(call => { });

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.AnomaliaErrors.CannotEnd);
        _anomaliaRepository.DidNotReceive().Update(Arg.Any<Anomalia>());    // Verifica se a anomalia não foi atualizada
        await _anomaliaRepository.UnitOfWork.DidNotReceive().CommitAsync(default);  // Verifica se o commit não foi chamado
   

    }

    [Fact]
    public async Task Handle_CommitChangesFails()
    {
        // Arrange
        var anomalia = Anomalia.Create(
             problemaId: ProblemaId.CreateNew().ToString(),
             setorId: SetorId.CreateNew().ToString(),
             restrita: false,
             dataDeAbertura: DateTime.Now,
             usuarioAberturaId: FuncionarioId.CreateNew().Value,
             questionamento: "Questionamento da anomalia",
             resultadoEsperado: "Resultado esperado da anomalia"
             ).Value;  //cria uma instancia válidade de anomalia

        var command = new EndAnomaliaCommand(anomalia.Id.ToString(), "considerações finais", FuncionarioId.CreateNew().Value, DateTime.Now);

        _anomaliaRepository.GetByIdAsync(anomalia.Id, default).Returns(anomalia);
        _anomaliaRepository.When(a => a.Update(anomalia)).Do(call => { });
        _anomaliaRepository.UnitOfWork.When(u => u.CommitAsync(default)).Do(call => { throw new Exception("Erro no commit"); }); // Simula falha na operação de commit


        // Act      
        Func<Task<Result>> action = async () => await _handler.Handle(command, default);

        // Assert     
        await action.Should().ThrowAsync<Exception>().WithMessage("Erro no commit");  // Verifica se a exceção correta foi lançada



    }


}
