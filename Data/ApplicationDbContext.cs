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
            base.OnModelCreating(modelBuilder);

            // Relacionamento 1:1 entre Pessoa e Voluntario
            modelBuilder.Entity<Voluntario>()
                .HasOne(v => v.Pessoa) // Voluntário tem uma Pessoa
                .WithOne() // Pessoa tem um único Voluntário
                .HasForeignKey<Voluntario>(v => v.CPF) // O CPF será a chave estrangeira
                .OnDelete(DeleteBehavior.Cascade); // Quando a pessoa for deletada, o voluntário também será deletado

            // Relacionamento entre Voluntário e Abrigo (1:N)
            modelBuilder.Entity<Voluntario>()
                .HasOne(v => v.Abrigo) // Voluntário tem um Abrigo
                .WithMany(a => a.Voluntarios) // Abrigo tem muitos voluntários
                .HasForeignKey(v => v.NomeAbrigo) // Relacionamento pelo nome do abrigo
                .OnDelete(DeleteBehavior.Cascade); // Deletar voluntários quando o abrigo for deletado

            // Relacionamento entre Aviso e Bairro (1:N)
            modelBuilder.Entity<Aviso>()
                .HasOne(a => a.Bairro) // Aviso tem um Bairro
                .WithMany() // Bairro pode ter muitos avisos
                .HasForeignKey(a => a.Bairro) // Relacionamento pelo bairro
                .OnDelete(DeleteBehavior.Restrict); // Evitar deleção em cascata de bairros
        }
    }
}
