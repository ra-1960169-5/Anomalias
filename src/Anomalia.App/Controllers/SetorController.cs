using Anomalias.App.Abstractions;
using Anomalias.App.Extensions;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Extensions;
using Anomalias.Application.Funcionarios.Query.GetAll;
using Anomalias.Application.Setores.Commands.Create;
using Anomalias.Application.Setores.Commands.Delete;
using Anomalias.Application.Setores.Commands.Update;
using Anomalias.Application.Setores.Query.Get;
using Anomalias.Application.Setores.Query.GetById;
using Anomalias.Application.ViewModels;
using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Identity.Authorization;
using Anomalias.Infrastructure.Identity.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Anomalias.App.Controllers;

[CustomAuthorize(EPermissions.GERENCIADOR)]
public class SetorController(IMediatorHandler mediator) : BaseController(mediator)
{
    [HttpGet("listar-setors")]
    public async Task<IActionResult> Index(int pg = 1, int ps = 10, string? search = null, string? sortColumn = null, string? sortOrder = null)
    {
        const string ACTION = "index";
        ViewBag.CurrentSort = sortColumn;
        ViewBag.CurrentFilter = search;
        ViewBag.CurrentSortOrder = sortOrder;
        var setoresQueryResult = await Mediator.SendCommandAsync(new GetSetorQuery(search, sortColumn, sortOrder));
        if (HasErrors(setoresQueryResult)) return RedirectToAction("Index", "Home");
        var setoresPagedViewModel = await PagedViewModel<SetorVM>.CreateAsync(
        setoresQueryResult.Value.ToViewModel(),
        pg,
        ps,
        ACTION,
        search,
        sortColumn,
        sortOrder);
        return View(setoresPagedViewModel);
    }

    [HttpGet("novo-setor")]
    public IActionResult Create()
    {
        return PartialView(viewName: "_Create");
    }

    [HttpPost("novo-setor")]
    public async Task<IActionResult> Create(NovoSetorVM request, CancellationToken cancellationToken)
    {
        var commandCreateSetor = new CreateSetorCommad(request.Descricao!);
        var resultCreateSetorCommand = await Mediator.SendCommandAsync(commandCreateSetor, cancellationToken);
        if (HasErrors(resultCreateSetorCommand)) return PartialView(viewName: "_Create");     
        TempData["Sucesso"] = "Cadastrado com sucesso!";
        return Json(data: new { success = true });
    }
    [HttpGet("editar-setor/{id}")]
    public async Task<IActionResult> Edit([ModelBinder(typeof(StrongerIdBinder))] SetorId id, CancellationToken cancellationToken)
    {
        var funcionarios = await Mediator.SendCommandAsync(new GetAllFuncionarioQuery(), cancellationToken);
        ViewBag.Funcionarios = funcionarios.Value.ToViewModel().Where(x => x.SetorId == id.ToString()); 
        var setor = await Mediator.SendCommandAsync(new GetByIdSetorQuery(id), cancellationToken);
        if (HasErrors(setor)) return RedirectToAction("Index", "Setor");
        return PartialView(viewName: "_Edit", setor.Value.ToViewModel());
    }

    [HttpPost("editar-setor/{id}")]
    public async Task<IActionResult> Edit([ModelBinder(typeof(StrongerIdBinder))] SetorId id, SetorVM request, CancellationToken cancellationToken)
    {
        if (id.ToString() != request.Id) return NotFound();
        var commandUpdateSetor = new UpdateSetorCommand(id, request.Descricao!, request.GestorId);
        var resultUpdateSetorCommand = await Mediator.SendCommandAsync(commandUpdateSetor, cancellationToken);
        if (HasErrors(resultUpdateSetorCommand))
        {
            var funcionarios = await Mediator.SendCommandAsync(new GetAllFuncionarioQuery(), cancellationToken);
          
            ViewBag.Funcionarios = funcionarios.Value.ToViewModel().Where(x=>x.SetorId==id.ToString());
            return PartialView(viewName: "_Edit", request);
        }
        TempData["Sucesso"] = "Atualizado com sucesso!";
        return Json(data: new { success = true });
    }

    [HttpGet("deletar-setor/{id}")]
    public async Task<IActionResult> Delete([ModelBinder(typeof(StrongerIdBinder))] SetorId id)
    {
        var setor = await Mediator.SendCommandAsync(new GetByIdSetorQuery(id));
        if (HasErrors(setor)) return RedirectToAction("Index", "Setor");
        return PartialView(viewName: "_Delete", setor.Value.ToViewModel());
    }

    [HttpPost("deletar-setor/{id}")]
    public async Task<IActionResult> Delete([ModelBinder(typeof(StrongerIdBinder))] SetorId id, SetorVM request, CancellationToken cancellationToken)
    {
        if (id.ToString() != request.Id) return NotFound();

        var commandDeleteSetor = new DeleteSetorCommand(id);
        var resultDeleteSetorCommand = await Mediator.SendCommandAsync(commandDeleteSetor, cancellationToken);
        if (HasErrors(resultDeleteSetorCommand)) return PartialView(viewName: "_Delete");

        TempData["Sucesso"] = "Deletado com sucesso!";
        return Json(data: new { success = true });
    }
}
