using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Problemas.Commands.Update;
public sealed record UpdateProblemaCommand(ProblemaId Id, string Descricao) : ICommand;
