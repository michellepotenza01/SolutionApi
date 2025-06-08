# Use a imagem base do .NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Use a imagem para build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo csproj e restaurar as dependências
COPY SolutionApi.csproj ./   # Ajustando o caminho para a raiz do diretório
RUN dotnet restore "SolutionApi.csproj"

# Copiar o restante dos arquivos da aplicação
COPY . . 

# Publicar a aplicação
WORKDIR /src/SolutionApi
RUN dotnet publish "SolutionApi.csproj" -c Release -o /app/publish

# Usar a imagem base para o container final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "SolutionApi.dll"]
