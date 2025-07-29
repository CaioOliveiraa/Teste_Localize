using CadastroEmpresas.Data;
using CadastroEmpresas.Dtos.Auth;
using CadastroEmpresas.Models;
using CadastroEmpresas.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CadastroEmpresas.Tests.Auth;

public class AuthServiceTests
{
    private ApplicationDbContext CriarDbContextEmMemoria()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private IConfiguration CriarConfiguracaoFalsa()
    {
        var settings = new Dictionary<string, string?>
        {
            { "Jwt:Key", "chave-testemunho-secreta-para-jwt-de-32-chars" },
            { "Jwt:Issuer", "CadastroEmpresas" },
            { "Jwt:Audience", "CadastroEmpresas" },
        };

        return new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
    }

    [Fact]
    public async Task RegistrarAsync_DeveCadastrarNovoUsuario()
    {
        var dbContext = CriarDbContextEmMemoria();
        var config = CriarConfiguracaoFalsa();
        var authService = new AuthService(dbContext, config);

        var novoUsuario = new UsuarioRegistroDto
        {
            Nome = "João da Silva",
            Email = "joao@email.com",
            Senha = "senha123",
        };

        var resultado = await authService.RegistrarAsync(novoUsuario);

        Assert.NotNull(resultado);
        Assert.Equal(novoUsuario.Email, resultado.Email);
    }

    [Fact]
    public async Task LoginAsync_DeveRetornarToken_SeLoginValido()
    {
        var dbContext = CriarDbContextEmMemoria();
        var config = CriarConfiguracaoFalsa();
        var authService = new AuthService(dbContext, config);

        var dtoCadastro = new UsuarioRegistroDto
        {
            Nome = "Maria",
            Email = "maria@email.com",
            Senha = "senha123",
        };

        await authService.RegistrarAsync(dtoCadastro);

        var loginDto = new UsuarioLoginDto { Email = "maria@email.com", Senha = "senha123" };

        var resultado = await authService.LoginAsync(loginDto);

        Assert.NotNull(resultado);
        Assert.False(string.IsNullOrWhiteSpace(resultado.Token));
    }

    [Fact]
    public async Task LoginAsync_DeveRetornarNull_SeEmailNaoExiste()
    {
        var dbContext = CriarDbContextEmMemoria();
        var config = CriarConfiguracaoFalsa();
        var authService = new AuthService(dbContext, config);

        var loginDto = new UsuarioLoginDto
        {
            Email = "naoexiste@email.com",
            Senha = "qualquercoisa",
        };

        var resultado = await authService.LoginAsync(loginDto);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task LoginAsync_DeveRetornarNull_SeSenhaIncorreta()
    {
        var dbContext = CriarDbContextEmMemoria();
        var config = CriarConfiguracaoFalsa();
        var authService = new AuthService(dbContext, config);

        await authService.RegistrarAsync(
            new UsuarioRegistroDto
            {
                Nome = "Carlos",
                Email = "carlos@email.com",
                Senha = "senha123",
            }
        );

        var loginDto = new UsuarioLoginDto { Email = "carlos@email.com", Senha = "senhaErrada" };

        var resultado = await authService.LoginAsync(loginDto);

        Assert.Null(resultado);
    }
}
