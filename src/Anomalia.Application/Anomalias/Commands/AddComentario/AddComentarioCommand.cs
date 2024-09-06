using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Application.ViewModels;


namespace Anomalias.Application.Anomalias.Commands.AddComentario;
public sealed record AddComentarioCommand(string Comentario, string AnomaliaId, Guid ComentaristaId, DateTime DataDoComentario, AnexoVM? Anexo = null) : ICommand;
