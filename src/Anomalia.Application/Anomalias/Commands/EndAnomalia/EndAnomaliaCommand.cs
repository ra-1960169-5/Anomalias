using Anomalias.Application.Abstractions.Messaging;

namespace Anomalias.Application.Anomalias.Commands.EndAnomalia;
public sealed record EndAnomaliaCommand(string AnomalaiId, string ConsideracoesFinais, Guid UsuarioEncerramentoId, DateTime DataDeEncerramento) : ICommand;