using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace Anomalias.App.Configuration;

public static class PresentationConfiguration
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllersWithViews(o => o.MensagensEmPortugues());
        services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());
        services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
        return services;
    }
    public static void UsePresentation(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/erro/500");
            app.UseStatusCodePagesWithRedirects("/erro/{0}");
            app.UseHsts();
        }
        app.UseResponseCompression();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseGlobalization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }

    private static MvcOptions MensagensEmPortugues(this MvcOptions o)
    {
        o.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "O valor preenchido é inválido para este campo!");
        o.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => "Este campo precisa ser preenchido!");
        o.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Este campo precisa ser preenchido!");
        o.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "É necessario que o body na requisição não esteja vazio");
        o.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor((x) => "O valor preenchido é invalido para este campo!");
        o.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "O valor preenchido é invalido para este campo!");
        o.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "O campo deve ser numérico!");
        o.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((x) => "O valor preenchido é invalido para este campo");
        o.ModelBindingMessageProvider.SetValueIsInvalidAccessor((x) => "O valor preenchido é invalido para este campo");
        o.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "O campo deve ser númerico!");
        o.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "Este campo precisa ser preenchido!");
        o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        return o;
    }

}
