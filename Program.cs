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

            // Adicionar servi�os ao cont�iner
            builder.Services.AddControllers();

            // Configura��o do Swagger/OpenAPI com metadados e vers�o
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PreventionAPI",
                    Version = "v1",
                    Description = "API para gerenciar informa��es sobre preven��o de eventos clim�ticos extremos.",
                    Contact = new OpenApiContact
                    {
                        Name = "Equipe SolutionAPI",
                        Email = "equipe@solutionapi.com",
                        Url = new Uri("https://solutionapi.com/contact")
                    }
                });
            });

            // Configura��o do contexto de banco de dados Oracle
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

            // Adicionar Razor Pages para renderizar as p�ginas din�micas
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configura��o do pipeline de requisi��es HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PreventionAPI v1");
                    c.RoutePrefix = string.Empty; // Swagger acess�vel em "/"
                });
            }

            // Habilitar redirecionamento HTTPS
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Mapear Controllers e Razor Pages
            app.MapControllers();
            app.MapRazorPages();

            // Configurar o cont�iner para escutar na porta correta para Docker
            app.Run("http://0.0.0.0:8080");
        }
    }
}
