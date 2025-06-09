#Dockerfile for .NET 8.0 application	
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /App

ENV PATH=$PATH:/root/.dotnet/tools
RUN dotnet tool install --global dotnet-ef

COPY . ./

RUN dotnet restore SolutionApi.sln

RUN dotnet publish SolutionApi.sln -c Release -o out


RUN groupadd -r appuser && useradd -r -g appuser -m appuser

USER appuser
ENV PATH=$PATH:/home/appuser/.dotnet/tools
RUN dotnet tool install --global dotnet-ef


USER root

WORKDIR /App/out

RUN chown -R appuser:appuser /App/out

EXPOSE 5000

USER appuser

ENTRYPOINT ["dotnet", "SolutionApi.dll"]