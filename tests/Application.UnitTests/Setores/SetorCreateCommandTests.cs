using Anomalias.Application.Abstractions.Repository;
using Anomalias.Application.Setores.Commands.Create;
using Anomalias.Domain.Entities;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Setores;
public class SetorCreateCommandTests
{
    
        private readonly ISetorRepository _setorRepository;
        private readonly CreateSetorCommandHandler _handler;

        public SetorCreateCommandTests()
        {
            // Arrange: Configura as dependências simuladas e o handler
            _setorRepository = Substitute.For<ISetorRepository>();
            _handler = new CreateSetorCommandHandler(_setorRepository);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenSetorIsCreatedSuccessfully()
        {
            // Arrange: Cria o comando de criação e simula um setor válido
            var command = new CreateSetorCommad("Descricao Teste");
            var setor = Setor.Create(command.Descricao.ToUpper());

        // Simula a adição do setor ao repositório
            _setorRepository.Add(setor.Value);
            _setorRepository.UnitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

            // Act: Executa o método Handle do handler
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert: Verifica se o resultado é um sucesso e se os métodos de repositório foram chamados
            result.IsSuccess.Should().BeTrue();
            _setorRepository.Received(1).Add(setor.Value);
            await _setorRepository.UnitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenSetorCreationFails()
        {
            // Arrange: Cria o comando de criação e simula um erro na criação do setor
            var command = new CreateSetorCommad(string.Empty);
            var errorExpected = DomainErrors.SetorErrors.Create;
            var setor = Result.Failure<Setor>(errorExpected);

            // Simula o resultado da criação falha
            _setorRepository.When(x => x.Add(Arg.Any<Setor>())).Do(x => { throw new Exception(); });

            // Act: Executa o método Handle do handler
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert: Verifica se o resultado é uma falha e se o repositório não recebeu a chamada de adição
            result.IsFailure.Should().BeTrue();
            result.Errors.Should().ContainSingle(e => e == errorExpected);
            _setorRepository.DidNotReceive().Add(Arg.Any<Setor>());
            await _setorRepository.UnitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenCommitFails()
        {
            // Arrange: Cria o comando de criação e simula uma falha no commit
            var command = new CreateSetorCommad("Descricao Teste");
            var setor = Setor.Create(command.Descricao.ToUpper());

        // Simula o comportamento de adicionar o setor ao repositório
             _setorRepository.Add(setor.Value);

            // Simula um erro no commit da unidade de trabalho
            _setorRepository.UnitOfWork.CommitAsync(Arg.Any<CancellationToken>())
                .Returns(Result.Failure(new Error("Commit", "Falha ao realizar operação de banco de dados!")));

            // Act: Executa o método Handle do handler        
           var result = await _handler.Handle(command, CancellationToken.None);
        // Assert: Verifica se o resultado é uma falha devido ao erro no commit    
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(new Error("Commit", "Falha ao realizar operação de banco de dados!"));
        _setorRepository.Received(1).Add(setor.Value);
        await _setorRepository.UnitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }
}

