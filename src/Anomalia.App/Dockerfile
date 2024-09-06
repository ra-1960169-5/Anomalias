#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Anomalia.App/Anomalias.App.csproj", "src/Anomalia.App/"]
COPY ["src/Anomalia.Infrastructure/Anomalias.Infrastructure.csproj", "src/Anomalia.Infrastructure/"]
COPY ["src/Anomalia.Application/Anomalias.Application.csproj", "src/Anomalia.Application/"]
COPY ["src/Anomalia.Domain/Anomalias.Domain.csproj", "src/Anomalia.Domain/"]
COPY ["src/Anomalia.Shared/Anomalias.Shared.csproj", "src/Anomalia.Shared/"]
RUN dotnet restore "./src/Anomalia.App/Anomalias.App.csproj"
COPY . .
WORKDIR "/src/src/Anomalia.App"
RUN dotnet build "./Anomalias.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Anomalias.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Anomalias.App.dll"]