using CadastroEmpresas.Dtos.Auth;
using CadastroEmpresas.Services;
using Microsoft.AspNetCore.Mvc;

namespace CadastroEmpresas.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registrar")]
    public async Task<ActionResult<UsuarioResponseDto>> Registrar(UsuarioRegistroDto dto)
    {
        var resposta = await _authService.RegistrarAsync(dto);
        if (resposta == null)
            return BadRequest("E-mail já cadastrado.");

        return Ok(resposta);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UsuarioResponseDto>> Login(UsuarioLoginDto dto)
    {
        var resposta = await _authService.LoginAsync(dto);
        if (resposta == null)
            return Unauthorized("E-mail ou senha inválidos.");

        Response.Cookies.Append(
            "token",
            resposta.Token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(1),
            }
        );

        return Ok(resposta);
    }
}
