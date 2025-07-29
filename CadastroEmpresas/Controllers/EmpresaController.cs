using System.Security.Claims;
using CadastroEmpresas.Dtos.Empresa;
using CadastroEmpresas.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CadastroEmpresas.Controllers;

[ApiController]
[Route("empresas")]
[Authorize]
public class EmpresaController : ControllerBase
{
    private readonly IEmpresaService _empresaService;

    public EmpresaController(IEmpresaService empresaService)
    {
        _empresaService = empresaService;
    }

    [HttpPost]
    public async Task<ActionResult<EmpresaResponseDto>> Cadastrar(
        [FromBody] CadastrarEmpresaDto dto
    )
    {
        int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var empresa = await _empresaService.CadastrarEmpresaPorCnpjAsync(dto.Cnpj, usuarioId);
        if (empresa == null)
            return BadRequest("Erro ao buscar dados da empresa.");

        return Ok(empresa);
    }

    [HttpGet]
    public async Task<ActionResult<List<EmpresaResponseDto>>> Listar()
    {
        int usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var empresas = await _empresaService.ListarEmpresasDoUsuarioAsync(usuarioId);
        return Ok(empresas);
    }
}
