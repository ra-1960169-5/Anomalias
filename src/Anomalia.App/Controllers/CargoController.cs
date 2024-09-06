using Anomalias.App.Abstractions;
using Anomalias.App.Extensions;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Cargos.Commands.CreateCargo;
using Anomalias.Application.Cargos.Commands.DeleteCargo;
using Anomalias.Application.Cargos.Commands.UpdateCargo;
using Anomalias.Application.Cargos.Query.Get;
using Anomalias.Application.Cargos.Query.GetById;
using Anomalias.Application.Extensions;
using Anomalias.Application.ViewModels;
using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Identity.Authorization;
using Anomalias.Infrastructure.Identity.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Anomalias.App.Controllers;

[CustomAuthorize(EPermissions.GERENCIADOR)]
public class CargoController(IMediatorHandler mediator) : BaseController(mediator)
{
    [HttpGet("listar-cargos")]
    public async Task<IActionResult> Index(int pg = 1, int ps = 5, string? search = null, string? sortColumn = null, string? sortOrder = null)
    {
        const string ACTION = "index";
        ViewBag.CurrentSort = sortColumn;
        ViewBag.CurrentFilter = search;
        ViewBag.CurrentSortOrder = sortOrder;
        var cargosQueryResult = await Mediator.SendCommandAsync(new GetCargosQuery(search, sortColumn, sortOrder));
        if (HasErrors(cargosQueryResult)) return RedirectToAction("Index", "Home");
        var cargosPagedViewModel = await PagedViewModel<CargoVM>.CreateAsync(
          cargosQueryResult.Value.ToViewModel(),
          pg,
          ps,
          ACTION,
          search,
          sortColumn,
          sortOrder);
        return View(cargosPagedViewModel);
    }


    [HttpGet("novo-cargo")]
    public IActionResult Create()
    {
        return PartialView(viewName: "_Create");
    }
    [HttpPost("novo-cargo")]
    public async Task<IActionResult> Create(CargoVM request, CancellationToken cancellationToken)
    {
        var commandCreateCargo = new CreateCargoCommand(request.Descricao!);
        var resultCreateCargoCommand = await Mediator.SendCommandAsync(commandCreateCargo, cancellationToken);
        if (HasErrors(resultCreateCargoCommand)) return PartialView(viewName: "_Create",request);
        TempData["Sucesso"] = "Cadastrado com sucesso!";
        return Json(data: new { success = true });
    }

    [HttpGet("editar-cargo/{id}")]
    public async Task<IActionResult> Edit([ModelBinder(typeof(StrongerIdBinder))] CargoId id)
    {
        var cargo = await Mediator.SendCommandAsync(new GetByIdCargoQuery(id));
        if (HasErrors(cargo)) return RedirectToAction("Index", "Cargo");
        return PartialView(viewName: "_Edit", cargo.Value.ToViewModel());
    }

    [HttpPost("editar-cargo/{id}")]
    public async Task<IActionResult> Edit([ModelBinder(typeof(StrongerIdBinder))] CargoId id, CargoVM request, CancellationToken cancellationToken)
    {
        if (id.ToString() != request.Id) return NotFound();

        var commandUpdateCargo = new UpdateCargoCommand(id, request.Descricao!);
        var resultUpdateCargoCommand = await Mediator.SendCommandAsync(commandUpdateCargo, cancellationToken);
        if (HasErrors(resultUpdateCargoCommand)) return PartialView(viewName: "_Edit");
        TempData["Sucesso"] = "Atualizado com sucesso!";
        return Json(data: new { success = true });
    }

    [HttpGet("deletar-cargo/{id}")]
    public async Task<IActionResult> Delete([ModelBinder(typeof(StrongerIdBinder))] CargoId id)
    {
        var cargo = await Mediator.SendCommandAsync(new GetByIdCargoQuery(id));
        if (HasErrors(cargo)) return RedirectToAction("Index", "Cargo");
        return PartialView(viewName: "_Delete", cargo.Value.ToViewModel());
    }

    [HttpPost("deletar-cargo/{id}")]
    public async Task<IActionResult> Delete([ModelBinder(typeof(StrongerIdBinder))] CargoId id, CargoVM request, CancellationToken cancellationToken)
    {
        if (id.ToString() != request.Id) return NotFound();

        var commandDeleteCargo = new DeleteCargoCommand(id);
        var resultDeleteCargoCommand = await Mediator.SendCommandAsync(commandDeleteCargo, cancellationToken);
        if (HasErrors(resultDeleteCargoCommand)) return PartialView(viewName: "_Delete");

        TempData["Sucesso"] = "Deletado com sucesso!";
        return Json(data: new { success = true });
    }
}
