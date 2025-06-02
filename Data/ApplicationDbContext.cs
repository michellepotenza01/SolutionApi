using Microsoft.EntityFrameworkCore;
using SolutionApi.Models;

namespace SolutionApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Definindo as tabelas do banco de dados
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Voluntario> Voluntarios { get; set; }
        public DbSet<Abrigo> Abrigos { get; set; }
        public DbSet<Aviso> Avisos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacionamento 1:1 entre Pessoa e Voluntário
            modelBuilder.Entity<Voluntario>()
                .HasOne(v => v.Pessoa)
                .WithMany()
                .HasForeignKey(v => v.CPF)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento entre Voluntário e Abrigo (1:N)
            modelBuilder.Entity<Voluntario>()
                .HasOne(v => v.Abrigo)
                .WithMany(a => a.Voluntarios)
                .HasForeignKey(v => v.NomeAbrigo)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento entre Aviso e Bairro (1:N) com Bairro sendo uma string
            modelBuilder.Entity<Aviso>()
                .Property(a => a.Bairro)
                .HasMaxLength(100); // Garantir que o campo Bairro tenha um tamanho adequado

            // Não há necessidade de um relacionamento físico no banco para "Bairro"
            // Apenas garantimos que a propriedade Bairro seja tratada como uma string simples.

            base.OnModelCreating(modelBuilder);
        }
    }
}
