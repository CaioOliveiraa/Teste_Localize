namespace CadastroEmpresas.Models;

public class Empresa
{
    public int Id { get; set; }

    public string NomeEmpresarial { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Situacao { get; set; } = string.Empty;
    public string Abertura { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string NaturezaJuridica { get; set; } = string.Empty;
    public string AtividadePrincipal { get; set; } = string.Empty;

    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Municipio { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
}
