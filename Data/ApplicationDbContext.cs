using Microsoft.EntityFrameworkCore;
using SolutionApi.Models;

namespace SolutionApi.Data
{
    /// <summary>
    /// Contexto do banco de dados que gerencia as entidades e suas interações com o banco de dados.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Construtor para o contexto do banco de dados.
        /// </summary>
        /// <param name="options">Opções de configuração do DbContext.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Representa a tabela de Pessoas no banco de dados.
        /// </summary>
        public DbSet<Pessoa> Pessoas { get; set; }

        /// <summary>
        /// Representa a tabela de Voluntários no banco de dados.
        /// </summary>
        public DbSet<Voluntario> Voluntarios { get; set; }

        /// <summary>
        /// Representa a tabela de Abrigos no banco de dados.
        /// </summary>
        public DbSet<Abrigo> Abrigos { get; set; }

        /// <summary>
        /// Representa a tabela de Avisos no banco de dados.
        /// </summary>
        public DbSet<Aviso> Avisos { get; set; }

        /// <summary>
        /// Configura os relacionamentos e restrições de dados no modelo de banco de dados.
        /// </summary>
        /// <param name="modelBuilder">Modelo de criação do banco de dados.</param>
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
