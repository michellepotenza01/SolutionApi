using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace SolutionApi.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
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
