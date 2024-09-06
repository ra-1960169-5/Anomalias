using Anomalias.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Anomalias.App.ViewComponets;

public class PaginateViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IPagedList paginated)
    {
        return View(paginated);
    }
}
