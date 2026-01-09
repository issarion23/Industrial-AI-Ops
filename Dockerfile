FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Industrial-AI-Ops.sln", "./"]

COPY ["src/Industrial-AI-Ops.Api/Industrial-AI-Ops.Api.csproj", "src/Industrial-AI-Ops.Api/"]
COPY ["src/Industrial-AI-Ops.Core/Industrial-AI-Ops.Core.csproj", "src/Industrial-AI-Ops.Core/"]
COPY ["src/Industrial-AI-Ops.Infrastructure/Industrial-AI-Ops.Infrastructure.csproj", "src/Industrial-AI-Ops.Infrastructure/"]
COPY ["src/Industrial-AI-Ops.ML/Industrial-AI-Ops.ML.csproj", "src/Industrial-AI-Ops.ML/"]

RUN dotnet restore "src/Industrial-AI-Ops.Api/Industrial-AI-Ops.Api.csproj"

COPY . .

WORKDIR "/src/src/Industrial-AI-Ops.Api"
RUN dotnet build "Industrial-AI-Ops.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Industrial-AI-Ops.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

# Создаём директорию для ML моделей
RUN mkdir -p /app/models

ENTRYPOINT ["dotnet", "Industrial-AI-Ops.Api.dll"]