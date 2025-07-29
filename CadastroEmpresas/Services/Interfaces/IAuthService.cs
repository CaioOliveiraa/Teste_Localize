using CadastroEmpresas.Dtos.Auth;

namespace CadastroEmpresas.Services;

public interface IAuthService
{
    Task<UsuarioResponseDto?> RegistrarAsync(UsuarioRegistroDto dto);
    Task<UsuarioResponseDto?> LoginAsync(UsuarioLoginDto dto);
}
