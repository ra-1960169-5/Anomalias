using Microsoft.AspNetCore.Mvc.Razor;

namespace Anomalias.App.Extensions;

public static class RazorExtensions
{

    public static string CorStatus(this RazorPage page, int input)
    {
        if (page is null) throw new ArgumentNullException(nameof(page));
        return input == 1 ? "bg-success" : "bg-danger";
    }

    public static string CorRestrita(this RazorPage page, bool input)
    {
        if (page is null) throw new ArgumentNullException(nameof(page));
        return input ? "bg-danger" : "bg-dark";
    }

    public static string Restrita(this RazorPage page, bool input)
    {
        if (page is null) throw new ArgumentNullException(nameof(page));
        return input ? "Sim" : "Não";
    }


    public static string CorLogin(this RazorPage page, bool input)
    {
        if (page is null) throw new ArgumentNullException(nameof(page));
        return input ? "" : "bg-danger";
    }
}