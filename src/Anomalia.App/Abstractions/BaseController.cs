using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Anomalias.App.Abstractions;
public abstract class BaseController(IMediatorHandler mediator) : Controller
{

    protected readonly IMediatorHandler Mediator = mediator;

    protected bool HasErrors(Result result)
    {
        if (result == null || result.IsSuccess) return false;


        foreach (var error in result.Errors)
        {

            ModelState.AddModelError(error.Code, error.Description);
        }
        return true;

    }


    protected bool HasErrors(FluentValidation.Results.ValidationResult result)
    {
        if (result == null || result.IsValid) return false;
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.ErrorCode, error.ErrorMessage);
        }
        return true;
    }


    protected bool HasErrorsList(IList<Result> results)
    {

        return results.Select(x => HasErrors(x)).Any(x => x is true);
    }
}
