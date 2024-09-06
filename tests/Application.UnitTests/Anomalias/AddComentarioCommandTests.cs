using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Anomalias.Commands.AddComentario;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Anomalias;
public class AddComentarioCommandTests
{
    private readonly IAnomaliaRepository _anomaliaRepository;
    private readonly AddComentarioCommandHandler _handler;

    public AddComentarioCommandTests()
    {
        _anomaliaRepository = Substitute.For<IAnomaliaRepository>();
        _handler = new AddComentarioCommandHandler(_anomaliaRepository);
    }

    [Fact]
    public async Task Handle_ShouldAddComentario_WhenDataIsValid()
    {
        // Arrange
        var anomalia = Anomalia.Create(
            problemaId: ProblemaId.CreateNew().ToString(),
            setorId: SetorId.CreateNew().ToString(),
            restrita: false,
            dataDeAbertura: DateTime.Now,
            usuarioAberturaId: FuncionarioId.CreateNew().Value,
            questionamento: "Questionamento da Anomalia",
            resultadoEsperado: "Resultado Esperado da Anomaila"
            ).Value;
            

        _anomaliaRepository.GetByIdAsync(Arg.Any<AnomaliaId>(), Arg.Any<CancellationToken>())
            .Returns(anomalia);

      

        var command = new AddComentarioCommand(
            AnomaliaId: anomalia.Id.ToString(),
            Comentario: "Comentário válido",
            ComentaristaId: FuncionarioId.CreateNew().Value,
            DataDoComentario: DateTime.Now

        );

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();  // Verifica se o resultado foi um sucesso
         _anomaliaRepository.Received(1).Update(anomalia);  // Verifica se a anomalia foi atualizada
        await _anomaliaRepository.UnitOfWork.Received(1).CommitAsync(cancellationToken);  // Verifica se o commit foi chamado
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAnomaliaNotFound()
    {
        // Arrange
        _anomaliaRepository.GetByIdAsync(Arg.Any<AnomaliaId>(), Arg.Any<CancellationToken>())
            .Returns(null as Anomalia);

        var command = new AddComentarioCommand(
            AnomaliaId: AnomaliaId.CreateNew().ToString(),
            Comentario: "Comentário válido",
            ComentaristaId: FuncionarioId.CreateNew().Value,
            DataDoComentario: DateTime.Now

        );
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Errors.Should().Contain(DomainErrors.AnomaliaErrors.NotFound);  // Verifica se o erro de não encontrado foi retornado
         _anomaliaRepository.DidNotReceive().Update(Arg.Any<Anomalia>());  // Verifica se a anomalia não foi atualizada
        await _anomaliaRepository.UnitOfWork.DidNotReceive().CommitAsync(cancellationToken);  // Verifica se o commit não foi chamado
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAddComentarioFails()
    {
        // Arrange
        var anomalia = Anomalia.Create(
          problemaId: ProblemaId.CreateNew().ToString(),
          setorId: SetorId.CreateNew().ToString(),
          restrita: false,
          dataDeAbertura: DateTime.Now,
          usuarioAberturaId: FuncionarioId.CreateNew().Value,
          questionamento: "Questionamento da Anomalia",
          resultadoEsperado: "Resultado Esperado da Anomaila"
          ).Value;

        _anomaliaRepository.GetByIdAsync(Arg.Any<AnomaliaId>(), Arg.Any<CancellationToken>())
            .Returns(anomalia);


        var command = new AddComentarioCommand(
        AnomaliaId: anomalia.Id.ToString(),
        Comentario: string.Empty,  // Comentário inválido para simular falha
        ComentaristaId: FuncionarioId.CreateNew().Value,
        DataDoComentario: DateTime.Now);

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();  // Verifica se o resultado foi uma falha
        result.Errors.Should().Contain(DomainErrors.ComentarioErrors.Create);  // Verifica a mensagem de erro retornada
         _anomaliaRepository.DidNotReceive().Update(Arg.Any<Anomalia>());  // Verifica se a anomalia não foi atualizada
        await _anomaliaRepository.UnitOfWork.DidNotReceive().CommitAsync(cancellationToken);  // Verifica se o commit não foi chamado
    }

    [Fact]
    public async Task Handle_ShouldAddComentarioWithAnexo_WhenDataIsValid()
    {
        // Arrange
        var anomalia = Anomalia.Create(
         problemaId: ProblemaId.CreateNew().ToString(),
         setorId: SetorId.CreateNew().ToString(),
         restrita: false,
         dataDeAbertura: DateTime.Now,
         usuarioAberturaId: FuncionarioId.CreateNew().Value,
         questionamento: "Questionamento da Anomalia",
         resultadoEsperado: "Resultado Esperado da Anomaila"
         ).Value;

        _anomaliaRepository.GetByIdAsync(Arg.Any<AnomaliaId>(), Arg.Any<CancellationToken>())
            .Returns(anomalia);

      
        
        var command = new AddComentarioCommand(
               AnomaliaId: anomalia.Id.ToString(),
               Comentario: "Comentário válido",
               ComentaristaId: FuncionarioId.CreateNew().Value,
               DataDoComentario: DateTime.Now,
               Anexo: new(ContentType: "application/pdf", FileName: "file.pdf", Dados: [0x01, 0x02, 0x03])// Simulação de um anexo válido
           );

        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();  // Verifica se o resultado foi um sucesso
         _anomaliaRepository.Received(1).Update(anomalia);  // Verifica se a anomalia foi atualizada
        await _anomaliaRepository.UnitOfWork.Received(1).CommitAsync(cancellationToken);  // Verifica se o commit foi chamado
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAddAnexoComentarioFails()
    {
        // Arrange
        var anomalia = Anomalia.Create(
          problemaId: ProblemaId.CreateNew().ToString(),
          setorId: SetorId.CreateNew().ToString(),
          restrita: false,
          dataDeAbertura: DateTime.Now,
          usuarioAberturaId: FuncionarioId.CreateNew().Value,
          questionamento: "Questionamento da Anomalia",
          resultadoEsperado: "Resultado Esperado da Anomaila"
          ).Value;
        _anomaliaRepository.GetByIdAsync(Arg.Any<AnomaliaId>(), Arg.Any<CancellationToken>())
            .Returns(anomalia);



        var command = new AddComentarioCommand(
               AnomaliaId: anomalia.Id.ToString(),
               Comentario: "Comentário válido",
               ComentaristaId: FuncionarioId.CreateNew().Value,
               DataDoComentario: DateTime.Now,
               Anexo: new(ContentType: string.Empty, FileName: string.Empty, Dados: [])// Simulação de falha no anexo  
           );
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.IsFailure.Should().BeTrue();  // Verifica se o resultado foi uma falha
        result.Errors.Should().Contain(DomainErrors.AnexoErrors.Create);  // Verifica a mensagem de erro retornada    
         _anomaliaRepository.DidNotReceive().Update(Arg.Any<Anomalia>());  // Verifica se a anomalia não foi atualizada
        await _anomaliaRepository.UnitOfWork.DidNotReceive().CommitAsync(cancellationToken);  // Verifica se o commit não foi chamado
    }
}
