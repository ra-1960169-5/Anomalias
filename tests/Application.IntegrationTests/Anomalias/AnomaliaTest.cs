using Anomalias.Application.Anomalias.Commands.AddComentario;
using Anomalias.Application.Anomalias.Commands.EndAnomalia;
using Anomalias.Application.Anomalias.Commands.RegisterAnomalia;
using Anomalias.Application.Anomalias.Query.GetByID;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Enums;
using Application.IntegrationTests.Fixtures;
using FluentAssertions;

namespace Application.IntegrationTests.Anomalias;
public class AnomaliaTest(CustomWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact(DisplayName = "DEVE REGISTRAR UMA ANOMALIA")]
    public async Task Test1()
    {     
        var problema = CreateProblema("ProblemaTeste");
        var setor = CreateSetor("SetorAnomaliaTeste");
        var setorFuncionario = CreateSetor("SetorFuncionarioTeste");
        var cargoFuncionario = CreateCargo("CargoTeste");
        var funcionario = CreateFuncionario("teste", "teste@email.com.br", setorFuncionario.Id.ToString(), cargoFuncionario.Id.ToString(), false);
        RegisterAnomaliaCommand registerAnomaliaCommand = new(problema.Id.ToString(), setor.Id.ToString(), false, DateTime.Now, funcionario.Id.Value, "questionamento", "resultado esperado");
        var resultRegisterAnomaliaCommand = await Mediator.SendCommandAsync(registerAnomaliaCommand, default);
        resultRegisterAnomaliaCommand.IsSuccess.Should().BeTrue();
        var anomaliaId = AnomaliaId.TryParse(resultRegisterAnomaliaCommand.Value)!.Value;
        var resultGetAnomalia = await Mediator.SendCommandAsync(new GetByIdAnomaliaQuery(anomaliaId));
        resultGetAnomalia.IsSuccess.Should().BeTrue();
        resultGetAnomalia.Value.Id.Should().BeEquivalentTo(anomaliaId.ToString());
        resultGetAnomalia.Value.Setor.Should().Be(setor.Descricao);
        resultGetAnomalia.Value.Problema.Should().Be(problema.Descricao);
        resultGetAnomalia.Value.DataAbertura.Should().Be(registerAnomaliaCommand.DataDeAbertura);
        resultGetAnomalia.Value.DataEncerramento.Should().BeNull();
        resultGetAnomalia.Value.Questionamento.Should().Be(registerAnomaliaCommand.Questionamento);
        resultGetAnomalia.Value.ResultadoEsperado.Should().Be(registerAnomaliaCommand.ResultadoEsperado);
        resultGetAnomalia.Value.Restrita.Should().Be(registerAnomaliaCommand.Restrita);
        resultGetAnomalia.Value.UsuarioAbertura.Should().Be(funcionario.Nome);
        resultGetAnomalia.Value.Status.Should().Be((int)EStatus.Andamento);
        resultGetAnomalia.Value.ConsideracoesFinais.Should().BeNullOrEmpty();
        resultGetAnomalia.Value.UsuarioEncerramento.Should().BeNullOrEmpty();
        resultGetAnomalia.Value.AnexoId.Should().BeNullOrEmpty();
        resultGetAnomalia.Value.NumeroRegistro.Should().BeGreaterThanOrEqualTo(1);
        resultGetAnomalia.Value.Comentarios.Should().BeNullOrEmpty();
    }

    [Fact(DisplayName = "DEVE ADCIONAR UM COMENTARIO A UMA ANOMALIA")]
    public async Task Test2()
    {
        var anomalia = CreateAnomalia();
        var setorFuncionario = CreateSetor("Comentarista");
        var cargoFuncionario = CreateCargo("Comentarista");
        var funcionario = CreateFuncionario("Comentarista", "comentarista@email.com.br", setorFuncionario.Id.ToString(), cargoFuncionario.Id.ToString(), false);
        AddComentarioCommand addComentarioCommand = new("comentario teste", anomalia.Id.ToString(), funcionario.Id.Value, DateTime.Now);
        var resultAddComentarioCommand = await Mediator.SendCommandAsync(addComentarioCommand, default);
        resultAddComentarioCommand.IsSuccess.Should().BeTrue();
        var resultGetAnomalia = await Mediator.SendCommandAsync(new GetByIdAnomaliaQuery(anomalia.Id));
        resultGetAnomalia.IsSuccess.Should().BeTrue();
        resultGetAnomalia.Value.Comentarios[0].ComentadoPor.Should().Be(funcionario.Nome);
        resultGetAnomalia.Value.Comentarios[0].Data.Should().Be(addComentarioCommand.DataDoComentario);
        resultGetAnomalia.Value.Comentarios[0].Descricao.Should().Be(addComentarioCommand.Comentario);
        resultGetAnomalia.Value.Comentarios[0].AnexoId.Should().BeNullOrEmpty();
    }

    [Fact(DisplayName = "DEVE ENCERRAR UM ANOMALIA")]
    public async Task Test3()
    {
        var anomalia = CreateAnomalia();
        var setorFuncionario = CreateSetor("Encerrador");
        var cargoFuncionario = CreateCargo("Encerrador");
        var funcionario = CreateFuncionario("Encerrador", "encerrador@email.com.br", setorFuncionario.Id.ToString(), cargoFuncionario.Id.ToString(), false);
        EndAnomaliaCommand endAnomaliaCommand = new(anomalia.Id.ToString(), "Teste de Encerramento", funcionario.Id.Value, DateTime.Now);
        var resultEndAnomaliaCommand = await Mediator.SendCommandAsync(endAnomaliaCommand, default);
        resultEndAnomaliaCommand.IsSuccess.Should().BeTrue();
        var resultGetAnomalia = await Mediator.SendCommandAsync(new GetByIdAnomaliaQuery(anomalia.Id));
        resultGetAnomalia.IsSuccess.Should().BeTrue();
        resultGetAnomalia.Value.ConsideracoesFinais.Should().Be(endAnomaliaCommand.ConsideracoesFinais);
        resultGetAnomalia.Value.UsuarioEncerramento.Should().Be(funcionario.Nome);
        resultGetAnomalia.Value.DataEncerramento.Should().Be(endAnomaliaCommand.DataDeEncerramento);
        resultGetAnomalia.Value.Status.Should().Be((int)EStatus.Encerrado);
    }
}
