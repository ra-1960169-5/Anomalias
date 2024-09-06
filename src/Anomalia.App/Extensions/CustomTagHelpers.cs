using Anomalias.Infrastructure.Identity.Authorization;
using Anomalias.Infrastructure.Identity.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Anomalias.App.Extensions;

[HtmlTargetElement("*", Attributes = "supress-by-permission")]

public class ApagaElementoByClaimTagHelper(IHttpContextAccessor contextAccessor) : TagHelper
{

    [HtmlAttributeName("supress-by-permission")]
    public EPermissions Permissions { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(output);

        var temAcesso = CustomAuthorization.ValidarPermission(contextAccessor.HttpContext!, Permissions);

        if (temAcesso) return;

        output.SuppressOutput();
    }
}


[HtmlTargetElement("*", Attributes = "disable-by-permission")]
public class DesabilitaLinkByPermissionTagHelper(IHttpContextAccessor contextAccessor) : TagHelper
{
    [HtmlAttributeName("disable-by-permission")]
    public EPermissions Permissions { get; set; }



    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(output);

        var temAcesso = CustomAuthorization.ValidarPermission(contextAccessor.HttpContext!, Permissions);

        if (temAcesso) return;

        output.Attributes.RemoveAll("href");
        output.Attributes.RemoveAll("data-model");
        output.Attributes.RemoveAll("data-bs-target");
        output.Attributes.RemoveAll("data-bs-toggle");
        output.Attributes.Add(new TagHelperAttribute("style", "cursor: not-allowed !important;"));
        output.Attributes.Add(new TagHelperAttribute("class", "cursor-not-allowed"));
        output.Attributes.Add(new TagHelperAttribute("title", "Você não tem permissão"));
    }
}