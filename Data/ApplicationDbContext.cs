using GerenciamentoDeCarrosAPI.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Carro> Carros { get; set; }
    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<CarroFavorito> CarrosFavoritos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarroFavorito>()
            .HasKey(cf => new { cf.PessoaId, cf.CarroId });

        modelBuilder.Entity<CarroFavorito>()
            .HasOne(cf => cf.Pessoa)
            .WithMany(p => p.CarrosFavoritos)
            .HasForeignKey(cf => cf.PessoaId);

        modelBuilder.Entity<CarroFavorito>()
            .HasOne(cf => cf.Carro)
            .WithMany()
            .HasForeignKey(cf => cf.CarroId);
    }
}