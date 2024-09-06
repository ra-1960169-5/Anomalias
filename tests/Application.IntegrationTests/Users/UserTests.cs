using Anomalias.Application.Users.Commands.CreateUser;
using Anomalias.Domain.Errors;
using Anomalias.Shared;
using Application.IntegrationTests.Fixtures;
using FluentAssertions;

namespace Application.IntegrationTests.Users;
public class UserTests(CustomWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private static readonly CreateUserCommand Command = new("teste@teste.com.br", "Teste@123");

    [Fact]
    public async Task Command_Should_ReturnError_WhenEmailIsInvalid()
    {

        // Arrange
        CreateUserCommand invalidCommand = Command with { Email = "invalid_email" };

        // Act
        Result result = await Mediator.SendCommandAsync(invalidCommand);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.EmailErrors.InvalidFormat);
    }

    [Fact]
    public async Task Command_Should_ReturnSucess_WhenCommandIsValid()
    {

        // Arrange
        CreateUserCommand isValidCommand = Command;

        // Act                     
        Result result = await Mediator.SendCommandAsync(isValidCommand);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
    }

    [Fact]
    public async Task Command_Should_ReturnError_WhenEmailIsNotUnique()
    {

        // Arrange
        CreateUserCommand isValidCommand = Command with {Email = "teste2@teste.com.br" };

        // Act      
                        await Mediator.SendCommandAsync(isValidCommand);
        Result result = await Mediator.SendCommandAsync(isValidCommand);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(DomainErrors.EmailErrors.EmailNotUnique);

    }

}
