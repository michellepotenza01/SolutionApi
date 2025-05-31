using Microsoft.EntityFrameworkCore;
using SolutionApi.Data;
using Microsoft.OpenApi.Models;

namespace SolutionApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicionar serviços ao contêiner
            builder.Services.AddControllers();

            // Configuração do Swagger/OpenAPI com metadados e versão
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PreventionAPI",
                    Version = "v1",
                    Description = "API para gerenciar informações sobre prevenção de eventos climáticos extremos.",
                    Contact = new OpenApiContact
                    {
                        Name = "Equipe SolutionAPI",
                        Email = "equipe@solutionapi.com",
                        Url = new Uri("https://solutionapi.com/contact")
                    }
                });
            });

            // Configuração do contexto de banco de dados Oracle
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

            // Adicionar Razor Pages para renderizar as páginas dinâmicas
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configuração do pipeline de requisições HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PreventionAPI v1");
                    c.RoutePrefix = string.Empty; // Swagger acessível em "/"
                });
            }

            // Habilitar redirecionamento HTTPS
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Mapear Controllers e Razor Pages
            app.MapControllers();
            app.MapRazorPages();

            // Configurar o contêiner para escutar na porta correta para Docker
            app.Run("http://0.0.0.0:8080");
        }
    }
}
