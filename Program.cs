using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SolutionApi.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do DbContext para Oracle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Adicionar servi�os ao cont�iner
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Adicionar suporte para convers�o de enums para string
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Configura��o do Swagger/OpenAPI com metadados e vers�o
builder.Services.AddEndpointsApiExplorer();  // Importante para Swagger detectar os controladores
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SolutionApi",
        Version = "v1",
        Description = "API para gerenciar informa��es sobre preven��o de eventos clim�ticos extremos.",
        Contact = new OpenApiContact
        {
            Name = "Equipe SolutionAPI",
            Email = "equipe@solutionapi.com",
            Url = new Uri("https://solutionapi.com/contact")
        }
    });

    // Habilitar anota��es para os controllers
    c.EnableAnnotations();  // Importante para garantir que os atributos de anota��es sejam respeitados

    // Adicionar tags para os controllers com base no nome do controlador
    c.TagActionsBy(api =>
    {
        var controllerName = api.ActionDescriptor.RouteValues["controller"];
        return controllerName != null ? new List<string> { controllerName } : new List<string>();
    });
});

var app = builder.Build();

// Configura��o do pipeline de requisi��es HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SolutionApi v1");
        c.RoutePrefix = string.Empty;  // Swagger acess�vel no root "/"
    });
}

app.UseHttpsRedirection();

// Mapear Controllers para garantir que eles sejam reconhecidos
app.MapControllers();  // Este m�todo � essencial para mapear os controladores

// Inicia o servidor
app.Run("http://localhost:8080");
