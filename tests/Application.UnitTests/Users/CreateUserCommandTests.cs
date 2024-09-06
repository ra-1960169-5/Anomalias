using Anomalias.Application.Abstractions.Services;
using Anomalias.Application.Users.Commands.CreateUser;
using Anomalias.Domain.Errors;
using Anomalias.Infrastructure.Identity.Errors;
using Anomalias.Shared;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTests.Users;
public class CreateUserCommandTests
{

    private static readonly CreateUserCommand Command = new("teste@teste.com.br", "Teste@123");
    private readonly CreateUserCommandHandler _handler;
    private readonly IIdentityService _identityServiceMock;
    
    

    public CreateUserCommandTests()
    {       
        _identityServiceMock = Substitute.For<IIdentityService>();
        _handler = new CreateUserCommandHandler(_identityServiceMock);
    }


    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailIsInvalid() {

        // Arrange
        CreateUserCommand invalidCommand = Command with { Email = "invalid_email" };

        // Act
        Result result = await _handler.Handle(invalidCommand, default);

        // Assert
        result.Errors.Should().Contain(DomainErrors.EmailErrors.InvalidFormat);

    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailIsNotUnique()
    {
       //Arrange
        _identityServiceMock.IsEmailUniqueAsync(Arg.Is<string>(e => e == Command.Email))
            .Returns(false);

        // Act
        Result result = await _handler.Handle(Command, default);

        // Assert
        result.Errors.Should().Contain(DomainErrors.EmailErrors.EmailNotUnique);
      
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenCreateSucceeds()
    {
        // Arrange
        _identityServiceMock.IsEmailUniqueAsync(Arg.Is<string>(e => e == Command.Email)).Returns(true);
        _identityServiceMock.RegisterUserAsync(Command.Email, Command.Password).Returns(Result.Success(Guid.NewGuid().ToString()));

        // Act
        Result result = await _handler.Handle(Command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenCreateFailure()
    {
        // Arrange
        _identityServiceMock.IsEmailUniqueAsync(Arg.Is<string>(e => e == Command.Email)).Returns(true);
        _identityServiceMock.RegisterUserAsync(Command.Email, Command.Password).Returns(Result.Failure<string>(IdentityErrors.User.UserCreate));

        // Act
        Result result = await _handler.Handle(Command, default);

        // Assert
        result.Errors.Should().Contain([IdentityErrors.User.UserCreate]);
    }


}
