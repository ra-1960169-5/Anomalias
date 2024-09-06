using Anomalias.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Anomalias.App.Controllers;

[Authorize]
public class HomeController() : Controller
{
    //private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    [Route("erro/{id:length(3,3)}")]
    public IActionResult Errors(int id)
    {
        var modelErro = new ErrorViewModel();
        if (id == 500)
        {
            modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
            modelErro.Titulo = "Ocorreu um erro!";
            modelErro.ErrorCode = id;
        }
        else if (id == 404)
        {
            modelErro.Mensagem = "A p�gina que est� procurando n�o existe! <br/>Em caso de d�vidas entre em contato com nosso suporte";
            modelErro.Titulo = "Ops! P�gina n�o encontrada.";
            modelErro.ErrorCode = id;

        }
        else if (id == 403)
        {
            modelErro.Mensagem = "Voc� n�o tem permiss�o.";
            modelErro.Titulo = "Acesso Negado";
            modelErro.ErrorCode = id;
        }
        else
        {
            return StatusCode(500);
        }
        return View("Error", modelErro);

    }




}
