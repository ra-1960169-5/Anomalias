using Anomalias.App.Abstractions;
using Anomalias.App.Extensions;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Cargos.Query.GetAll;
using Anomalias.Application.Extensions;
using Anomalias.Application.Funcionarios.Commands.DisableById;
using Anomalias.Application.Funcionarios.Commands.EnableById;
using Anomalias.Application.Funcionarios.Commands.Update;
using Anomalias.Application.Funcionarios.Query.Get;
using Anomalias.Application.Funcionarios.Query.GetById;
using Anomalias.Application.Setores.Query.GetAll;
using Anomalias.Application.ViewModels;
using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Identity.Authorization;
using Anomalias.Infrastructure.Identity.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Anomalias.App.Controllers;

[CustomAuthorize(EPermissions.GERENCIADOR)]
public class FuncionarioController(IMediatorHandler mediator) : BaseController(mediator)
{

    [HttpGet("listar-funcionarios")]
    public async Task<IActionResult> Index(int pg = 1, int ps = 5, string? search = null, string? sortColumn = null, string? sortOrder = null)
    {
        const string ACTION = "index";
        ViewBag.CurrentSort = sortColumn;
        ViewBag.CurrentFilter = search;
        ViewBag.CurrentSortOrder = sortOrder;
        var funcionarioQueryResult = await Mediator.SendCommandAsync(new GetFuncionariosQuery(search, sortColumn, sortOrder));
        if (HasErrors(funcionarioQueryResult)) return RedirectToAction("Index", "Home");
        var funcionarioPagedViewModel = await PagedViewModel<FuncionarioVM>.CreateAsync(
         funcionarioQueryResult.Value.ToViewModel(),
         pg,
         ps,
         ACTION,
         search,
         sortColumn,
         sortOrder);
        return View(funcionarioPagedViewModel);

    }

    [HttpGet("editar-funcionario/{id}")]
    public async Task<IActionResult> Edit([ModelBinder(typeof(StrongerIdBinder))] FuncionarioId id, CancellationToken cancellationToken)
    {
        var cargos = await Mediator.SendCommandAsync(new GetAllCargosQuery(), cancellationToken);
        var setores = await Mediator.SendCommandAsync(new GetAllSetoresQuery(), cancellationToken);
        ViewBag.Cargos = cargos.Value.ToViewModel();
        ViewBag.Setores = setores.Value.ToViewModel();
        var funcionario = await Mediator.SendCommandAsync(new GetByIdFuncionarioQuery(id), cancellationToken);
        if (HasErrors(funcionario)) return RedirectToAction("Index", "Funcionario");
        var funcionarioVM = funcionario.Value.ToViewModel();
        return PartialView(viewName: "_Edit", funcionarioVM);
    }


    [HttpPost("editar-funcionario/{id}")]
    public async Task<IActionResult> Edit([ModelBinder(typeof(StrongerIdBinder))] FuncionarioId id, FuncionarioVM request, CancellationToken cancellationToken)
    {
        if (id.ToString() != request.Id) return NotFound();
        var commandUpdateFuncionario = new UpdateFuncionarioCommand(id, request.Nome, request.SetorId, request.CargoId, request.Ativo, request.Gestor);
        var resultUpdateFuncionarioCommand = await Mediator.SendCommandAsync(commandUpdateFuncionario, cancellationToken);
        if (HasErrors(resultUpdateFuncionarioCommand))
        {
            var cargos = await Mediator.SendCommandAsync(new GetAllCargosQuery(), cancellationToken);
            var setores = await Mediator.SendCommandAsync(new GetAllSetoresQuery(), cancellationToken);
            ViewBag.Cargos = cargos.Value.ToViewModel();
            ViewBag.Setores = setores.Value.ToViewModel();
            return PartialView(viewName: "_Edit", request);
        }
        TempData["Sucesso"] = "Atualizado com sucesso!";
        return Json(data: new { success = true });
    }

    [HttpGet("desativar-funcionario/{id}")]
    public async Task<IActionResult> Disable([ModelBinder(typeof(StrongerIdBinder))] FuncionarioId id, CancellationToken cancellationToken)
    {

        var commandDisableByIdFuncionario = new DisableByIdFuncionarioCommand(id);
        var resultDisableByIdFuncionarioCommand = await Mediator.SendCommandAsync(commandDisableByIdFuncionario, cancellationToken);
        HasErrors(resultDisableByIdFuncionarioCommand);
        return RedirectToAction("Index", "Funcionario");
    }

    [HttpGet("ativar-funcionario/{id}")]
    public async Task<IActionResult> Enable([ModelBinder(typeof(StrongerIdBinder))] FuncionarioId id, CancellationToken cancellationToken)
    {
        var commandEnableByIdFuncionario = new EnableByIdFuncionarioCommand(id);
        var resultEnableByIdFuncionarioCommand = await Mediator.SendCommandAsync(commandEnableByIdFuncionario, cancellationToken);
        HasErrors(resultEnableByIdFuncionarioCommand);
        return RedirectToAction("Index", "Funcionario");
    }
}
