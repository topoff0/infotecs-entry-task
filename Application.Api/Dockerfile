FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Application.sln", "."]
COPY ["Application.Api/Application.Api.csproj", "Application.Api/"]
COPY ["Application.Core/Application.Core.csproj", "Application.Core/"]
COPY ["Application.Data/Application.Data.csproj", "Application.Data/"]
COPY ["Application.Services/Application.Services.csproj", "Application.Services/"]
RUN dotnet restore "Application.Api/Application.Api.csproj"
COPY . .
WORKDIR "/src/Application.Api"
RUN dotnet build "Application.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Application.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Application.Api.dll"]
