using CadastroEmpresas.Dtos.Empresa;

namespace CadastroEmpresas.Services.Interfaces;

public interface IEmpresaService
{
    Task<EmpresaResponseDto?> CadastrarEmpresaPorCnpjAsync(string cnpj, int usuarioId);
    Task<List<EmpresaResponseDto>> ListarEmpresasDoUsuarioAsync(
        int usuarioId,
        int pagina,
        int quantidade
    );
}
