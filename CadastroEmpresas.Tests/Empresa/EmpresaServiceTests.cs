using System.Net;
using System.Net.Http;
using System.Text;
using CadastroEmpresas.Data;
using CadastroEmpresas.Dtos.Empresa;
using CadastroEmpresas.Models;
using CadastroEmpresas.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Xunit;

namespace CadastroEmpresas.Tests.EmpresaTests;

public class EmpresaServiceTests
{
    private ApplicationDbContext CriarDbContextEmMemoria()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private IHttpClientFactory CriarHttpClientFactoryFalso(string jsonRetorno)
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonRetorno, Encoding.UTF8, "application/json"),
                }
            );

        var client = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://www.receitaws.com.br/"),
        };

        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        return mockFactory.Object;
    }

    [Fact]
    public async Task CadastrarEmpresaPorCnpjAsync_DeveCadastrarEmpresa_SeApiRetornarSucesso()
    {
        // Arrange
        var dbContext = CriarDbContextEmMemoria();

        var respostaFalsa =
            @"{
            ""status"": ""OK"",
            ""nome"": ""Empresa Exemplo Ltda"",
            ""fantasia"": ""Empresa Exemplo"",
            ""cnpj"": ""12345678000195"",
            ""situacao"": ""ATIVA"",
            ""abertura"": ""01/01/2020"",
            ""tipo"": ""MATRIZ"",
            ""natureza_juridica"": ""206-2 - Sociedade Empresária Limitada"",
            ""atividade_principal"": [{""text"": ""Comércio varejista""}],
            ""logradouro"": ""Rua A"",
            ""numero"": ""123"",
            ""complemento"": ""Sala 2"",
            ""bairro"": ""Centro"",
            ""municipio"": ""Cidade"",
            ""uf"": ""PR"",
            ""cep"": ""80000000""
        }";

        var httpFactory = CriarHttpClientFactoryFalso(respostaFalsa);
        var service = new EmpresaService(dbContext, httpFactory);

        // Act
        var resultado = await service.CadastrarEmpresaPorCnpjAsync("12345678000195", 1);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("12345678000195", resultado.Cnpj);
        Assert.Equal("Empresa Exemplo Ltda", resultado.NomeEmpresarial);

        var empresaNoBanco = await dbContext.Empresas.FirstOrDefaultAsync();
        Assert.NotNull(empresaNoBanco);
        Assert.Equal("Empresa Exemplo", empresaNoBanco.NomeFantasia);
    }

    [Fact]
    public async Task CadastrarEmpresaPorCnpjAsync_DeveLancarExcecao_SeApiFalhar()
    {
        // Arrange
        var dbContext = CriarDbContextEmMemoria();
        var respostaInvalida = @"{ ""status"": ""ERROR"" }";
        var httpFactory = CriarHttpClientFactoryFalso(respostaInvalida);
        var service = new EmpresaService(dbContext, httpFactory);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() =>
            service.CadastrarEmpresaPorCnpjAsync("00000000000000", 1)
        );

        Assert.Equal("Erro ao buscar dados da ReceitaWS.", ex.Message);
    }

    [Fact]
    public async Task ListarEmpresasDoUsuarioAsync_DeveRetornarApenasEmpresasDoUsuario()
    {
        // Arrange
        var dbContext = CriarDbContextEmMemoria();

        dbContext.Empresas.AddRange(
            new Empresa
            {
                Cnpj = "111",
                NomeEmpresarial = "Empresa A",
                UsuarioId = 1,
            },
            new Empresa
            {
                Cnpj = "222",
                NomeEmpresarial = "Empresa B",
                UsuarioId = 1,
            },
            new Empresa
            {
                Cnpj = "333",
                NomeEmpresarial = "Empresa C",
                UsuarioId = 2,
            }
        );

        await dbContext.SaveChangesAsync();

        var httpFactoryMock = new Mock<IHttpClientFactory>();
        var service = new EmpresaService(dbContext, httpFactoryMock.Object);

        // Act
        var resultado = await service.ListarEmpresasDoUsuarioAsync(1, 1, 10);

        // Assert
        Assert.Equal(2, resultado.Count);
        Assert.All(
            resultado,
            e => Assert.Contains(e.NomeEmpresarial, new[] { "Empresa A", "Empresa B" })
        );
    }
}
