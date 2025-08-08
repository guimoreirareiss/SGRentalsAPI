using Microsoft.EntityFrameworkCore;
using SGRentalsAPI;
using SGRentalsAPI.Models;

namespace SGRentalsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets para suas entidades
        public DbSet<Empresa> Empresas => Set<Empresa>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Socio> Socios => Set<Socio>();
        public DbSet<Endereco> Enderecos => Set<Endereco>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura a relação um-para-muitos entre Empresa e Usuario
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Empresa)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(u => u.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configura a relação um-para-muitos entre Empresa e Socio
            modelBuilder.Entity<Socio>()
                .HasOne(s => s.Empresa)
                .WithMany(e => e.Socios)
                .HasForeignKey(s => s.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configura a relação um-para-um entre Empresa e Endereco
            modelBuilder.Entity<Empresa>()
                .HasOne(e => e.Endereco)
                .WithOne()
                .HasForeignKey<Empresa>(e => e.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
