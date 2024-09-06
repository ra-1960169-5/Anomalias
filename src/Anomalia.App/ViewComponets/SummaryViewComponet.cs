using Microsoft.AspNetCore.Mvc;

namespace Anomalias.App.ViewComponets;

public class SummaryViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {

        return View("Default");
    }

}
