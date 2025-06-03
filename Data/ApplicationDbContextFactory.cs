using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SolutionApi.Data
{
    /// <summary>
    /// Fábrica para criação do contexto do banco de dados em tempo de design.
    /// Usado para gerenciar as migrações do banco de dados.
    /// </summary>
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <summary>
        /// Cria uma instância do ApplicationDbContext.
        /// Este método é chamado em tempo de design, por exemplo, durante a execução de migrações.
        /// </summary>
        /// <param name="args">Argumentos fornecidos na linha de comando.</param>
        /// <returns>Uma instância configurada de ApplicationDbContext.</returns>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Usando a configuração do appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())  // Pega o diretório atual
                .AddJsonFile("appsettings.json")  // Carrega o arquivo de configurações
                .Build();

            // Obtém a string de conexão para o banco de dados
            var connectionString = configuration.GetConnectionString("OracleConnection");

            // Configura a conexão com o Oracle
            optionsBuilder.UseOracle(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
