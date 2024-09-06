using Anomalias.App.Abstractions;
using Anomalias.App.Extensions;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Extensions;
using Anomalias.Application.Problemas.Commands.Create;
using Anomalias.Application.Problemas.Commands.Delete;
using Anomalias.Application.Problemas.Commands.Update;
using Anomalias.Application.Problemas.Query.Get;
using Anomalias.Application.Problemas.Query.GetById;
using Anomalias.Application.ViewModels;
using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Identity.Authorization;
using Anomalias.Infrastructure.Identity.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Anomalias.App.Controllers;

[CustomAuthorize(EPermissions.GERENCIADOR)]
public class ProblemaController(IMediatorHandler mediator) : BaseController(mediator)
{
    [HttpGet("listar-problemas")]
    public async Task<IActionResult> Index(int pg = 1, int ps = 5, string? search = null, string? sortColumn = null, string? sortOrder = null)
    {
        const string ACTION = "index";
        ViewBag.CurrentSort = sortColumn;
        ViewBag.CurrentFilter = search;
        ViewBag.CurrentSortOrder = sortOrder;
        var problemasQueryResult = await Mediator.SendCommandAsync(new GetProblemaQuery(search, sortColumn, sortOrder));
        if (HasErrors(problemasQueryResult)) return RedirectToAction("Index", "Home");

        var problemaPagedViewModel = await PagedViewModel<ProblemaVM>.CreateAsync(
          problemasQueryResult.Value.ToViewModel(),
          pg,
          ps,
          ACTION,
          search,
          sortColumn,
          sortOrder);

        return View(problemaPagedViewModel);
    }

    [HttpGet("novo-problema")]
    public IActionResult Create()
    {
        return PartialView(viewName: "_Create");
    }

    [HttpPost("novo-problema")]
    public async Task<IActionResult> Create(ProblemaVM request, CancellationToken cancellationToken)
    {
        var commandCreateProblema = new CreateProblemaCommad(request.Descricao!);
        var resultCreateProblemaCommand = await Mediator.SendCommandAsync(commandCreateProblema, cancellationToken);
        if (HasErrors(resultCreateProblemaCommand)) return PartialView(viewName: "_Create");
        TempData["Sucesso"] = "Cadastrado com sucesso!";
        return Json(data: new { success = true });
    }
    [HttpGet("editar-problema/{id}")]
    public async Task<IActionResult> Edit([ModelBinder(typeof(StrongerIdBinder))] ProblemaId id)
    {
        var problema = await Mediator.SendCommandAsync(new GetByIdProblemaQuery(id));
        if (HasErrors(problema)) return RedirectToAction("Index", "Problema");
        return PartialView(viewName: "_Edit", problema.Value.ToViewModel());
    }

    [HttpPost("editar-problema/{id}")]
    public async Task<IActionResult> Edit([ModelBinder(typeof(StrongerIdBinder))] ProblemaId id, ProblemaVM request, CancellationToken cancellationToken)
    {
        if (id.ToString() != request.Id) return NotFound();
        var commandUpdateProblema = new UpdateProblemaCommand(id, request.Descricao!);
        var resultUpdateProblemaCommand = await Mediator.SendCommandAsync(commandUpdateProblema, cancellationToken);
        if (HasErrors(resultUpdateProblemaCommand)) return PartialView(viewName: "_Edit");
        TempData["Sucesso"] = "Atualizado com sucesso!";
        return Json(data: new { success = true });
    }

    [HttpGet("deletar-problema/{id}")]
    public async Task<IActionResult> Delete([ModelBinder(typeof(StrongerIdBinder))] ProblemaId id)
    {
        var problema = await Mediator.SendCommandAsync(new GetByIdProblemaQuery(id));
        if (HasErrors(problema)) return RedirectToAction("Index", "Problema");
        return PartialView(viewName: "_Delete", problema.Value.ToViewModel());
    }

    [HttpPost("deletar-problema/{id}")]
    public async Task<IActionResult> Delete([ModelBinder(typeof(StrongerIdBinder))] ProblemaId id, ProblemaVM request, CancellationToken cancellationToken)
    {
        if (id.ToString() != request.Id) return NotFound();
        var commandDeleteProblema = new DeleteProblemaCommand(id);
        var resultDeleteProblemaCommand = await Mediator.SendCommandAsync(commandDeleteProblema, cancellationToken);
        if (HasErrors(resultDeleteProblemaCommand)) return PartialView(viewName: "_Delete",request);
        TempData["Sucesso"] = "Deletado com sucesso!";
        return Json(data: new { success = true });
    }
}
