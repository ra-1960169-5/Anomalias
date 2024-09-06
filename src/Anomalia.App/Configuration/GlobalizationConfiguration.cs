using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Anomalias.App.Configuration;

public static class GlobalizationConfiguration
{
    public static IApplicationBuilder UseGlobalization(this IApplicationBuilder app)
    {
        var defaultCulture = new CultureInfo("pt-BR");
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(defaultCulture),
            SupportedCultures = [defaultCulture],
            SupportedUICultures = [defaultCulture]
        };
        app.UseRequestLocalization(localizationOptions);
        return app;
    }
}
