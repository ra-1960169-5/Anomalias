using Anomalias.Application.Abstractions.Messaging;
using Anomalias.Domain.Entities;
using Anomalias.Infrastructure.Persistence.Data;
using Application.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Application.IntegrationTests;
public abstract class BaseIntegrationTest:IClassFixture<CustomWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly IMediatorHandler Mediator;
    private readonly ApplicationDbContext _context;
    protected BaseIntegrationTest(CustomWebAppFactory factory) {        
        _scope = factory.Services.CreateScope();
        Mediator = _scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
        _context = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
    public static IFormFile CreateFormFile()
    {
        var name = "anexo";
        var fileName = "anexo.jpg";
        var content = "teste";
        var contentType = "image/jpeg";

        byte[] bytes = Encoding.UTF8.GetBytes(content);

        return new FormFile(
                 baseStream: new MemoryStream(bytes),
                 baseStreamOffset: 0,
                 length: bytes.Length,
                 name: name,
                 fileName: fileName
             )
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };

    }

    public Problema CreateProblema(string descricao) {
    
        var problema = Problema.Create(descricao).Value;
        _context.Problemas.Add(problema);
        _context.SaveChanges();
        return problema;
    }

    public Setor CreateSetor(string descricao)
    {
        var setor = Setor.Create(descricao).Value;
        _context.Setores.Add(setor);
        _context.SaveChanges();
        return setor;
    }

    public Cargo CreateCargo(string descricao)
    {
        var cargo = Cargo.Create(descricao).Value;
        _context.Cargos.Add(cargo);
        _context.SaveChanges();
        return cargo;
    }

    public Funcionario CreateFuncionario(string nome,string email,string setorId,string cargoId,bool gestor) {  
        var funcionario = Funcionario.Create(nome, email, setorId, cargoId, gestor).Value;       
        _context.Funcionarios.Add(funcionario);
        _context.SaveChanges();
        return funcionario;
    }

    public Anomalia CreateAnomalia() {
              
        var problema = CreateProblema("Teste");
        var setorAnomalia = CreateSetor("Anomalia");
        var setorFuncionario = CreateSetor("Teste");
        var cargoFuncionario = CreateCargo("Teste");
        var funcionario = CreateFuncionario("teste", "teste@email.com.br", setorFuncionario.Id.ToString(), cargoFuncionario.Id.ToString(), false);
        var anomalia =  Anomalia.Create(problema.Id.ToString(),setorAnomalia.Id.ToString(),false,DateTime.Now,funcionario.Id.Value,"questionamento","resultado esperado").Value;
        _context.Anomalias.Add(anomalia);
        _context.SaveChanges();
        return anomalia;
    }
}
