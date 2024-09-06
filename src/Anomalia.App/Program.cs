using Anomalias.App;
using Anomalias.App.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterDependencies(builder.Configuration,builder.Environment);
var app = builder.Build();
app.UsePresentation(builder.Environment);
app.Run();
public partial class Program{}