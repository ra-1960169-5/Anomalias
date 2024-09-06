using Anomalias.App.Abstractions;
using Anomalias.App.Extensions;
using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.Abstractions.Services;
using Anomalias.Application.Anexos.Query.GetById;
using Anomalias.Application.Anomalias.Commands.AddComentario;
using Anomalias.Application.Anomalias.Commands.EndAnomalia;
using Anomalias.Application.Anomalias.Commands.RegisterAnomalia;
using Anomalias.Application.Anomalias.Query.GetAll;
using Anomalias.Application.Anomalias.Query.GetByID;
using Anomalias.Application.Anomalias.Query.GetBySetor;
using Anomalias.Application.Anomalias.Query.GetByUser;
using Anomalias.Application.Extensions;
using Anomalias.Application.Problemas.Query.GetAll;
using Anomalias.Application.Setores.Query.GetAll;
using Anomalias.Application.ViewModels;
using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Identity.Authorization;
using Anomalias.Infrastructure.Identity.Enum;
using Microsoft.AspNetCore.Mvc;


namespace Anomalias.App.Controllers;

[CustomAuthorize(EPermissions.ANOMALIA)]
public class AnomaliaController(IMediatorHandler mediator, IDateTimeProvider dateTimeProvider, IUserContext userContext) : BaseController(mediator)
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IUserContext _userContext = userContext;

    [HttpGet("registrar-anomalia")]
    public async Task<IActionResult> RegisterAnomalia(CancellationToken cancellationToken)
    {
      
        var problemas = await Mediator.SendCommandAsync(new GetAllProblemasQuery(), cancellationToken);
        var setores = await Mediator.SendCommandAsync(new GetAllSetoresQuery(), cancellationToken);

        return View(new RegisterAnomaliaVM() { Problemas = problemas.Value.ToViewModel(), Setores = setores.Value.ToViewModel() });
    }


    [HttpPost("registrar-anomalia")]
    public async Task<IActionResult> RegisterAnomalia(RegisterAnomaliaVM request, CancellationToken cancellationToken)
    {
             
        var commandRegisterAnomalia = new RegisterAnomaliaCommand(request.ProblemaId, request.SetorId, request.Restrita, _dateTimeProvider.Now, _userContext.UserId, request.Questionamento, request.ResultadoEsperado, request.AnexoVM);
        var commandRegisterAnomaliaResult = await Mediator.SendCommandAsync(commandRegisterAnomalia, cancellationToken);
        if (HasErrors(commandRegisterAnomaliaResult)) return View(request with {
            Setores=Mediator.SendCommandAsync(new GetAllSetoresQuery(), cancellationToken).GetAwaiter().GetResult().Value.ToViewModel(),
            Problemas = Mediator.SendCommandAsync(new GetAllProblemasQuery(), cancellationToken).GetAwaiter().GetResult().Value.ToViewModel()
        });
        return RedirectToAction("DetailsAnomalia", "Anomalia", new { id = commandRegisterAnomaliaResult.Value });
    }


    [HttpGet("detalhes-anomalia/{id}")]
    public async Task<IActionResult> DetailsAnomalia([ModelBinder(typeof(StrongerIdBinder))] AnomaliaId id)
    {        
        var anomaliaQueryResult = await Mediator.SendCommandAsync(new GetByIdAnomaliaQuery(id));
        if (HasErrors(anomaliaQueryResult)) return View();
        return View(anomaliaQueryResult.Value.ToViewModel());
    }


    [HttpGet("consultar-anomalia")]
    public IActionResult ConsultAnomalia()
    {
        return View(new ConsultAnomaliaVM());
    }


    [HttpPost("consultar-anomalia")]
    public async Task<IActionResult> ConsultAnomalia(ConsultAnomaliaVM request, CancellationToken cancellationToken)
    {
        var allAnomaliaQueryResult = await Mediator.SendCommandAsync(new GetAllAnomaliaQuery(_userContext.UserId, request.DataInicial, request.DataFinal, request.Status), cancellationToken);
        var setorAnomaliaQueryResult = await Mediator.SendCommandAsync(new GetBySetorAnomaliaQuery(_userContext.UserId, request.DataInicial, request.DataFinal, request.Status), cancellationToken);
        var userAnomaliaQueryResult = await Mediator.SendCommandAsync(new GetByUserAnomaliaQuery(_userContext.UserId, request.DataInicial, request.DataFinal, request.Status), cancellationToken);

        if (HasErrorsList([allAnomaliaQueryResult, setorAnomaliaQueryResult, userAnomaliaQueryResult])) return View(request);

        request = request with
        {
            Anomalias = allAnomaliaQueryResult.Value.Select(x => x.ToViewModel()).ToList(),
            AnomaliasSetor = setorAnomaliaQueryResult.Value.Select(x => x.ToViewModel()).ToList(),
            AnomaliasUser = userAnomaliaQueryResult.Value.Select(x => x.ToViewModel()).ToList()
        };
        return View(request);
    }


    [HttpGet("adicionar-comentario/{id}")]
    public IActionResult AddComentario([ModelBinder(typeof(StrongerIdBinder))] AnomaliaId id)
    {

        return View(new AddComentarioVM { AnomaliaId = id.ToString()});
    }

    [HttpPost("adicionar-comentario/{id}")]
    public async Task<IActionResult> AddComentario(AddComentarioVM request, [ModelBinder(typeof(StrongerIdBinder))] AnomaliaId id, CancellationToken cancellationToken)
    {
        if (request.AnomaliaId != id.ToString()) return NotFound();      
        var addComentarioCommand = new AddComentarioCommand(request.Descricao, request.AnomaliaId, _userContext.UserId, _dateTimeProvider.Now, request.AnexoVM);
        var addComentarioCommandResult = await Mediator.SendCommandAsync(addComentarioCommand, cancellationToken);
        if (HasErrors(addComentarioCommandResult)) return View(request);
        return RedirectToAction("DetailsAnomalia", "Anomalia", new { id });

    }

    public async Task<IActionResult> DownloadAnexo([ModelBinder(typeof(StrongerIdBinder))] AnexoId id, CancellationToken cancellationToken)
    {
        var anexoQueryResult = await Mediator.SendCommandAsync(new GetByIdAnexoQuery(id), cancellationToken);
               
        return HasErrors(anexoQueryResult) is false
          ? File(anexoQueryResult.Value.Dados, anexoQueryResult.Value.ContentType, anexoQueryResult.Value.FileName)
          : NotFound();
    }


    [HttpGet("finalizar-anomalia/{id}")]
    public async Task<IActionResult> EndAnomalia([ModelBinder(typeof(StrongerIdBinder))] AnomaliaId id)
    {
        var anomaliaQueryResult = await Mediator.SendCommandAsync(new GetByIdAnomaliaQuery(id));
        if (anomaliaQueryResult.IsFailure || anomaliaQueryResult.Value is null) return NotFound();
        return View(new EndAnomaliaVM { AnomaliaId = anomaliaQueryResult.Value.Id.ToString(), NumeroRegistro = anomaliaQueryResult.Value.NumeroRegistro.ToString() });
    }

    [HttpPost("finalizar-anomalia/{id}")]
    public async Task<IActionResult> EndAnomalia(EndAnomaliaVM request, [ModelBinder(typeof(StrongerIdBinder))] AnomaliaId id, CancellationToken cancellationToken)
    {
        if (request.AnomaliaId != id.ToString()) return NotFound();
        var endAnomaliaCommand = new EndAnomaliaCommand(request.AnomaliaId, request.ConsideracoesFinais, _userContext.UserId, _dateTimeProvider.Now);
        var endAnomaliaCommandResult = await Mediator.SendCommandAsync(endAnomaliaCommand, cancellationToken);
        if (HasErrors(endAnomaliaCommandResult)) return View(request);
        return RedirectToAction("DetailsAnomalia", "Anomalia", new { id });

    }
}
