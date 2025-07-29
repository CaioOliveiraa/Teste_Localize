using System.Text.RegularExpressions;
using CadastroEmpresas.Data;
using CadastroEmpresas.Dtos.Empresa;
using CadastroEmpresas.Models;
using CadastroEmpresas.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CadastroEmpresas.Services;

public class EmpresaService : IEmpresaService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public EmpresaService(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<EmpresaResponseDto?> CadastrarEmpresaPorCnpjAsync(string cnpj, int usuarioId)
    {
        var cnpjNumeros = Regex.Replace(cnpj, @"[^\d]", "");

        if (cnpjNumeros.Length != 14)
            return null;

        var empresaExistente = await _context.Empresas.FirstOrDefaultAsync(e =>
            e.Cnpj == cnpjNumeros && e.UsuarioId == usuarioId
        );

        if (empresaExistente != null)
            return MapearParaDto(empresaExistente);

        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"https://www.receitaws.com.br/v1/cnpj/{cnpjNumeros}");

        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();
        var dados = JsonConvert.DeserializeObject<ReceitaWsResponse>(content);

        if (dados == null || dados.Status?.ToLower() != "ok")
            throw new Exception("Erro ao buscar dados da ReceitaWS.");

        var empresa = new Empresa
        {
            NomeEmpresarial = dados.Nome ?? "",
            NomeFantasia = dados.Fantasia ?? "",
            Cnpj = cnpjNumeros,
            Situacao = dados.Situacao ?? "",
            Abertura = dados.Abertura ?? "",
            Tipo = dados.Tipo ?? "",
            NaturezaJuridica = dados.NaturezaJuridica ?? "",
            AtividadePrincipal = dados.AtividadePrincipal?.FirstOrDefault()?.Texto ?? "",
            Logradouro = dados.Logradouro ?? "",
            Numero = dados.Numero ?? "",
            Complemento = dados.Complemento ?? "",
            Bairro = dados.Bairro ?? "",
            Municipio = dados.Municipio ?? "",
            Uf = dados.Uf ?? "",
            Cep = dados.Cep ?? "",
            UsuarioId = usuarioId,
        };

        _context.Empresas.Add(empresa);
        await _context.SaveChangesAsync();

        return MapearParaDto(empresa);
    }

    public async Task<List<EmpresaResponseDto>> ListarEmpresasDoUsuarioAsync(
        int usuarioId,
        int pagina,
        int quantidade
    )
    {
        var empresas = await _context
            .Empresas.Where(e => e.UsuarioId == usuarioId)
            .OrderBy(e => e.NomeFantasia)
            .Skip((pagina - 1) * quantidade)
            .Take(quantidade)
            .ToListAsync();

        return empresas.Select(MapearParaDto).ToList();
    }

    private EmpresaResponseDto MapearParaDto(Empresa empresa)
    {
        return new EmpresaResponseDto
        {
            NomeEmpresarial = empresa.NomeEmpresarial,
            NomeFantasia = empresa.NomeFantasia,
            Cnpj = empresa.Cnpj,
            Situacao = empresa.Situacao,
            Abertura = empresa.Abertura,
            Tipo = empresa.Tipo,
            NaturezaJuridica = empresa.NaturezaJuridica,
            AtividadePrincipal = empresa.AtividadePrincipal,
            Logradouro = empresa.Logradouro,
            Numero = empresa.Numero,
            Complemento = empresa.Complemento,
            Bairro = empresa.Bairro,
            Municipio = empresa.Municipio,
            Uf = empresa.Uf,
            Cep = empresa.Cep,
        };
    }
}
