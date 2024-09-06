using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Anomalias.Commands.RegisterAnomalia;
using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Persistence.Repository;
using Anomalias.Shared;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Anomalias;
public class RegisterAnomaliaCommandTests
{
    private readonly IAnomaliaRepository _anomaliaRepository; 
    private readonly RegisterAnomaliaCommandHandler _handler;
   
    public RegisterAnomaliaCommandTests()
    {
        _anomaliaRepository = Substitute.For<IAnomaliaRepository>();
        _handler = new RegisterAnomaliaCommandHandler(_anomaliaRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAnomaliaIsCreatedSuccessfully()
    {
        // Arrange
        var command = new RegisterAnomaliaCommand(
            ProblemaId: ProblemaId.CreateNew().ToString(),
            SetorId: SetorId.CreateNew().ToString(),
            Restrita: false,
            DataDeAbertura: DateTime.Now,
            UsuarioAberturaId: FuncionarioId.CreateNew().Value,
            Questionamento: "Questionamento da Anomalia",
            ResultadoEsperado: "Resultado Esperado da Anomaila"
            );
        var cancellationToken = CancellationToken.None;
   
        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();  //Verifica se o resultado foi um sucesso       
        _anomaliaRepository.Received(1).Add(Arg.Any<Anomalia>()); // Verifica se a anomalia foi adicionada ao repositório
        await _anomaliaRepository.UnitOfWork.Received(1).CommitAsync(cancellationToken); //Verifica se o commit foi chamado
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenAnomaliaCreationFails()
    {
        // Arrange
        var command = new RegisterAnomaliaCommand(
             ProblemaId: ProblemaId.CreateNew().ToString(),
             SetorId: SetorId.CreateNew().ToString(),
             Restrita: false,
             DataDeAbertura: DateTime.Now,
             UsuarioAberturaId: FuncionarioId.CreateNew().Value,
             Questionamento: string.Empty,// Dados inválidos que causam falha na criação da anomalia
             ResultadoEsperado: "Resultado Esperado da Anomaila"
             );
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();  // Verifica se o resultado foi uma falha
        _anomaliaRepository.DidNotReceive().Add(Arg.Any<Anomalia>());  // Verifica se a anomalia não foi adicionada ao repositório
        await _anomaliaRepository.UnitOfWork.DidNotReceive().CommitAsync(cancellationToken);  // Verifica se o commit não foi chamado
      
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenAnexoAdditionFails()
    {
        // Arrange       
        var command = new RegisterAnomaliaCommand(
            ProblemaId: ProblemaId.CreateNew().ToString(),
            SetorId: SetorId.CreateNew().ToString(),
            Restrita: false,
            DataDeAbertura: DateTime.Now,
            UsuarioAberturaId: FuncionarioId.CreateNew().Value,
            Questionamento: "O que está acontecendo?",
            ResultadoEsperado: "Solução esperada",
            Anexo:new(ContentType: string.Empty, FileName: string.Empty, Dados: [])// Simulação de falha no anexo  
            );
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();  // Verifica se o resultado foi uma falha
        _anomaliaRepository.DidNotReceive().Add(Arg.Any<Anomalia>());  // Verifica se a anomalia não foi adicionada ao repositório
        await _anomaliaRepository.UnitOfWork.DidNotReceive().CommitAsync(cancellationToken);  // Verifica se o commit não foi chamado
    }

    [Fact]
    public async Task Handle_ShouldRegisterAnomaliaWithAnexo_WhenDataIsValid()
    {
        // Arrange
        var command = new RegisterAnomaliaCommand(
             ProblemaId: ProblemaId.CreateNew().ToString(),
             SetorId: SetorId.CreateNew().ToString(),
             Restrita: false,
             DataDeAbertura: DateTime.Now,
             UsuarioAberturaId: FuncionarioId.CreateNew().Value,
             Questionamento: "O que está acontecendo?",
             ResultadoEsperado: "Solução esperada",
             Anexo: new(ContentType: "application/pdf", FileName: "file.pdf", Dados: [0x01, 0x02, 0x03])// Simulação de um anexo válido
             );
        var cancellationToken = CancellationToken.None;

        // Simula o sucesso na operação de commit
        _anomaliaRepository.UnitOfWork.CommitAsync(cancellationToken).Returns(Result.Success());

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();  // Verifica se o resultado foi um sucesso
         _anomaliaRepository.Received(1).Add(Arg.Any<Anomalia>());  // Verifica se a anomalia foi adicionada ao repositório
        await _anomaliaRepository.UnitOfWork.Received(1).CommitAsync(cancellationToken);  // Verifica se o commit foi chamado
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCommitFails()
    {
        // Arrange
        var command = new RegisterAnomaliaCommand(
            ProblemaId: ProblemaId.CreateNew().ToString(),
            SetorId: SetorId.CreateNew().ToString(),
            Restrita: false,
            DataDeAbertura: DateTime.Now,
            UsuarioAberturaId: FuncionarioId.CreateNew().Value,
            Questionamento: "O que está acontecendo?",
            ResultadoEsperado: "Solução esperada",
            Anexo:null //Nenhum Anexo
            );
        var cancellationToken = CancellationToken.None;

        // Simula falha na operação de commit
        _anomaliaRepository.UnitOfWork.CommitAsync(cancellationToken).Returns(Result.Failure(new Error("Commit", "Falha ao realizar operação de banco de dados!")));

        // Act      
        var result = await _handler.Handle(command, cancellationToken);

        // Assert      
        // Verifica se a exceção correta foi lançada
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(new Error("Commit", "Falha ao realizar operação de banco de dados!"));
    }


}
