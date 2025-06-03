using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SolutionApi.Data;
using Swashbuckle.AspNetCore.Annotations;

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
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;

    });

// Adicionar o EndpointsApiExplorer para Swagger detectar os controladores
builder.Services.AddEndpointsApiExplorer();

// Configura��o do Swagger/OpenAPI com metadados e vers�o
builder.Services.AddSwaggerGen(c =>
{
    // Definindo o SwaggerDoc
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
    c.EnableAnnotations();
});

var app = builder.Build();

// Configura��o do pipeline de requisi��es HTTP
if (app.Environment.IsDevelopment())
{
    // Ativa o Swagger e a UI do Swagger para testes
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SolutionApi v1");
        c.RoutePrefix = string.Empty;  // Swagger UI na URL raiz (http://localhost:5000)
    });
}

// Mapeando Controllers para garantir que eles sejam reconhecidos pelo Swagger
Console.WriteLine("Mapeando Controllers...");
app.MapControllers();

// Inicia o servidor na porta 5000
app.Run("http://localhost:5000");  // Usando a porta 5000
