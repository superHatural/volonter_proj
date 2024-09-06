FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/VolunterProg.API/VolunterProg.API.csproj", "src/VolunterProg.API/"]
COPY ["src/VolunterProg.Application/VolunterProg.Application.csproj", "src/VolunterProg.Application/"]
COPY ["src/VolunterProg.Domain/VolunterProg.Domain.csproj", "src/VolunterProg.Domain/"]
COPY ["src/VolunterProg.Infrastructure/VolunterProg.Infrastructure.csproj", "src/VolunterProg.Infrastructure/"]
RUN dotnet restore "src/VolunterProg.API/VolunterProg.API.csproj"
COPY . .
WORKDIR "/src/src/VolunterProg.API"
RUN dotnet build "VolunterProg.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "VolunterProg.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VolunterProg.API.dll"]
