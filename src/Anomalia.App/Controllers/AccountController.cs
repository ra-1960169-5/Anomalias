using Anomalias.App.Abstractions;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Cargos.Query.GetAll;
using Anomalias.Application.Extensions;
using Anomalias.Application.Funcionarios.Commands.CreateFuncionario;
using Anomalias.Application.Setores.Query.GetAll;
using Anomalias.Application.Users.Commands.CreateUser;
using Anomalias.Application.Users.Commands.DeleteUser;
using Anomalias.Application.Users.Commands.LoginUser;
using Anomalias.Application.Users.Commands.LogoutUser;
using Anomalias.Application.Users.Commands.RecoverPasswordUser;
using Anomalias.Application.Users.Commands.ResetPasswordUser;
using Anomalias.Application.ViewModels;
using Anomalias.Infrastructure.Identity.Authorization;
using Anomalias.Infrastructure.Identity.Enum;
using Anomalias.Infrastructure.Identity.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Anomalias.App.Controllers;
public class AccountController(IMediatorHandler mediator) : BaseController(mediator)
{
    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserVM request, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        var commandLoginUser = new LoginUserComand(request.Email, request.Password);
        var resultLoginUserCommand = await Mediator.SendCommandAsync(commandLoginUser);
        if (resultLoginUserCommand.IsSuccess) return RedirectToLocal(returnUrl);
        if (resultLoginUserCommand.Errors.Contains(IdentityErrors.User.IsLockedOut))return RedirectToAction("Lockout");
        if (HasErrors(resultLoginUserCommand)) return View(request);
        return View(request);
    }

    [HttpGet("nova-conta")]
    [CustomAuthorize(EPermissions.GERENCIADOR)]
    public async Task<IActionResult> Register(CancellationToken cancellationToken)
    {
        var cargos = await Mediator.SendCommandAsync(new GetAllCargosQuery(), cancellationToken);
        var setores = await Mediator.SendCommandAsync(new GetAllSetoresQuery(), cancellationToken);
        return View(new NovoFuncionarioVM() { Cargos = cargos.Value.ToViewModel(), Setores = setores.Value.ToViewModel() });
    }

    [HttpPost("nova-conta")]
    [CustomAuthorize(EPermissions.GERENCIADOR)]
    public async Task<IActionResult> Register(NovoFuncionarioVM request, CancellationToken cancellationToken)
    {
        var cargos = await Mediator.SendCommandAsync(new GetAllCargosQuery(), cancellationToken);
        var setores = await Mediator.SendCommandAsync(new GetAllSetoresQuery(), cancellationToken);
        request.Setores = setores.Value.ToViewModel();
        request.Cargos = cargos.Value.ToViewModel();
        var commandCreateUser = new CreateUserCommand(request.Email, request.Password);
        var resultCreateUserCommand = await Mediator.SendCommandAsync(commandCreateUser, cancellationToken);
        if (HasErrors(resultCreateUserCommand)) return View(request);
        var commandCreateFuncionario = new CreateFuncionarioCommand(resultCreateUserCommand.Value, request.Nome, request.Email, request.SetorId, request.CargoId, request.Gestor);
        var resultCreateFuncionarioCommand = await Mediator.SendCommandAsync(commandCreateFuncionario, cancellationToken);
        if (HasErrors(resultCreateFuncionarioCommand))
        {
            var commandDeleteUser = new DeleteUserCommand(resultCreateUserCommand.Value);
            var resultDeleteCommand = await Mediator.SendCommandAsync(commandDeleteUser, cancellationToken);
            if (!HasErrors(resultDeleteCommand))
                return View(request);
            throw new Exception("Erro ao deletar usuario");
        }
        TempData["Sucesso"] = "Usuário registrado com sucesso";
        return RedirectToAction("Index", "Funcionario");
    }

    [Authorize]
    [HttpGet("sair")]
    public async Task<IActionResult> Logout()
    {
        var commandLogoutUser = new LogoutUserCommand();
        await Mediator.SendCommandAsync(commandLogoutUser);    
        return RedirectToAction("Login", "Account");
    }



    [HttpGet("acesso-negado")]
    [AllowAnonymous]
    public IActionResult AccessDenied() => View();

    [HttpGet("esqueceu-a-senha")]
    [AllowAnonymous]
    public IActionResult ForgotPassword() => View();

    [HttpPost("esqueceu-a-senha")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model,CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);
        var commandRecoverPasswordUser = new RecoverPasswordUserCommand(model.Email);
        var resultRecoverPasswordUser = await Mediator.SendCommandAsync(commandRecoverPasswordUser, cancellationToken);
        if (HasErrors(resultRecoverPasswordUser)) return View(model);
            TempData["Sucesso"] = "Informações enviada por e-mail";
        return RedirectToAction("Login", "Account");
    }

    [HttpGet("reset-da-senha")]
    [AllowAnonymous]
    public IActionResult ResetPassword(string? code = null) => code == null ? View("Error") : View();


    [HttpPost("reset-da-senha")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordVM model,CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);
        var commandResetPasswordUser = new ResetPasswordUserCommand(model.Email,model.Code,model.Password);
        var resultResetPasswordUser = await Mediator.SendCommandAsync(commandResetPasswordUser, cancellationToken);
        if (resultResetPasswordUser.IsSuccess)
        {
            TempData["Sucesso"] = "Senha atualizada";
            return RedirectToAction("Login", "Account");
        }
        HasErrors(resultResetPasswordUser);
        return View();
    }

    private IActionResult RedirectToLocal(string? returnUrl) =>
        Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index", "Home");




}
