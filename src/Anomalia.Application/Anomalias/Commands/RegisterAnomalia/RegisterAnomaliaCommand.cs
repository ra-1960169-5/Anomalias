using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.ViewModels;

namespace Anomalias.Application.Anomalias.Commands.RegisterAnomalia;
public sealed record RegisterAnomaliaCommand(string ProblemaId, string SetorId, bool Restrita, DateTime DataDeAbertura, Guid UsuarioAberturaId, string Questionamento, string ResultadoEsperado, AnexoVM? Anexo = null) : ICommand<string>;
