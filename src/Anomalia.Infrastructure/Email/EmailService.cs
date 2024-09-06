using Anomalias.Application.Abstractions.Services;
using Anomalias.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Anomalias.Infrastructure.Email;
public class EmailService(IOptions<EmailSettings> mailSettings) : IEmailService
{
    private readonly EmailSettings _mailSettings = mailSettings.Value;
  

    public Task SendAnomaliaResgistredEmailAsync(Anomalia anomalia, CancellationToken cancellationToken)
    {
        List<(string Nome, string Email)> emails = [(anomalia.FuncionarioAbertura!.Nome, anomalia.FuncionarioAbertura!.Email), .. anomalia.Setor!.Funcionarios!.Select(x => (x.Nome, x.Email))];
          emails.ForEach(async x => await  SendEmailAsync(TempleteMail(anomalia, (x.Nome, x.Email))));
       
        return Task.CompletedTask;
    }

    public Task SendAnomaliaTerminatedEmailAsync(Anomalia anomalia, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SendComentarioResgistredEmailAsync(Comentario comentario, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task SendEmailAsync(MailMessage mail)
    {
        await ConfigureSmtp().SendMailAsync(mail);       
    }

    private SmtpClient ConfigureSmtp()
    {
        var smtpClient = new SmtpClient(_mailSettings.Host, _mailSettings.Port)
        {
            Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            //EnableSsl = true

        };

        return smtpClient;
    }

    private MailMessage TempleteMail(Anomalia anomalia, (string Nome, string Email) Destinatario)
    {
        string REMENTENTE = _mailSettings.Mail;
        Uri callbackUrl = new($"/detalhes-anomalia/{anomalia.Id}");
        string body = $@"
                            <html>
                            <head>
                                <meta http-equiv='content-type' content='text/html; charset=utf-8'>
                                <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
                                <meta property='og:title' content='Email'>
                                <title>Email</title>
                            </head>
                            <body text='#000000' bgcolor='#FFFFFF'>
                                <div class='moz-forward-container'>
                                    <br> <br> <br> <br>
                                    <table id='mkt' style='border:30px solid #e7e2dc;' width='630' border='0' cellspacing='0' cellpadding='0'
                                        bgcolor='#ffffff' align='center'>
                                        <tbody>
                                            <tr style='background-color: #941316;'>
                                                <td colspan='3' class='mkt-column-full mkt-img-full' width='630' height='91'>
                                                    <span style='display:block;height:0px;'>
                                                        <div style='display: block; ' moz-do-not-send='true' width='630' height='91' border='0'>
                                                            <h1 style='text-align: center;'>ANOMALIAS</h1>
                                                        </div>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='3' class='mkt-column-full mkt-img-full' id='mkt-banner-td' width='630'> <br> </td>
                                            </tr>
                                            <tr>
                                                <td colspan='3' height='20'><br></td>
                                            </tr>
                                            <tr>
                                                <td class='lateral-space' width='25'><br></td>
                                                <td id='mkt-content' data-li='' stidx='0' width='580'>
                                                    <div class='mkt-edit' style='margin:0 25px 0 25px;'>
                                                        <div class='mkt-edit-buttons'><span class='mkt-edit-edit mkt-edit-button fleft'></span><span
                                                                class='mkt-edit-remove mkt-edit-button fright'></span>
                                                        </div>
                                                        <table style='background-color:#ffffff' class='mkt-column-full' width='580' border='0'
                                                            cellspacing='0' cellpadding='0' align='center'>
                                                            <tbody>
                                                                <tr>
                                                                    <td class='mkt-column-full mkt-editable' width='580'
                                                                        style='font-family: Calibri Light; font-size: 15px; text-align: justify'>
                                                                    </td>
                                                                    <p>Caro <b>{Destinatario.Nome}</b>,</p>
                                                                    <p>Acabou de ser registrado uma nova Anomalia. Abaixo você pode verificar os
                                                                        dados.</p>
                                                                    <p><b>Área Verificada: </b>{anomalia.Setor!.Descricao} </p>
                                                                    <p><b>Problema: </b>{anomalia.Problema!.Descricao} </p>
                                                                    <p><b>Descrição: </b>{anomalia.Questionamento} </p>
                                                                    <p><b> Data e Hora do Chamado: </b>{anomalia.DataDeAbertura:F} </p>
                                                                    <p> Para acessar o registro direto do portal, <b><a href='{callbackUrl}'> clique aqui </a></b></p>
                                                                    <br>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </td>
                                                <td class='lateral-space' width='25'><br></td>
                                            </tr>
                                            <tr style='background-color: #941316;'>
                                                <td colspan='3' width='630' height='72'></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <br>
                                </div>
                            </body>
                            </html>";
        string subject = $"Uma nova Anomalia foi Registrada com Nº {anomalia.Numero}";
        MailMessage mailMessage = new(REMENTENTE, Destinatario.Email, subject, body);
                    mailMessage.IsBodyHtml = true;
        return mailMessage;
    }

    public async Task SendRecoverPasswordEmailAsync(string email, string link)
    {
        string REMENTENTE = _mailSettings.Mail;
        MailMessage mailMessage = new(REMENTENTE, email, "Redefinir Senha", $"Por gentileza, para redefinir sua senha <a href='{link}'>clique aqui</a>");
        mailMessage.IsBodyHtml = true;
        await SendEmailAsync(mailMessage);

    }


    
}



