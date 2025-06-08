# Use uma imagem base do .NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Use a imagem para build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SolutionApi/SolutionApi.csproj", "SolutionApi/"]
RUN dotnet restore "SolutionApi/SolutionApi.csproj"
COPY . . 
WORKDIR "/src/SolutionApi"
RUN dotnet build "SolutionApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SolutionApi.csproj" -c Release -o /app/publish

# Copie o resultado para o container final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SolutionApi.dll"]
