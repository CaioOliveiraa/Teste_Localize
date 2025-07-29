using CadastroEmpresas.Data;
using CadastroEmpresas.Dtos.Auth;
using CadastroEmpresas.Models;
using CadastroEmpresas.Utils;
using Microsoft.EntityFrameworkCore;

namespace CadastroEmpresas.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<UsuarioResponseDto?> RegistrarAsync(UsuarioRegistroDto dto)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return null;

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Token = TokenService.GerarToken(usuario, _config),
        };
    }

    public async Task<UsuarioResponseDto?> LoginAsync(UsuarioLoginDto dto)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            return null;

        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Token = TokenService.GerarToken(usuario, _config),
        };
    }
}
