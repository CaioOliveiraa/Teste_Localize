using CadastroEmpresas.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroEmpresas.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Empresa> Empresas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Usuario>()
            .HasMany(u => u.Empresas)
            .WithOne(e => e.Usuario!)
            .HasForeignKey(e => e.UsuarioId);

        base.OnModelCreating(modelBuilder);
    }
}
