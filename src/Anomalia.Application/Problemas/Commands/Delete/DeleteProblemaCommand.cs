using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Domain.Entities;

namespace Anomalias.Application.Problemas.Commands.Delete;


public sealed record DeleteProblemaCommand(ProblemaId Id) : ICommand;