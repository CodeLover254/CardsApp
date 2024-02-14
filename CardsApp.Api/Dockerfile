FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CardsApp.Api/CardsApp.Api.csproj", "CardsApp.Api/"]
COPY ["CardsApp.Domain/CardsApp.Domain.csproj", "CardsApp.Domain/"]
COPY ["CardsApp.Application/CardsApp.Application.csproj", "CardsApp.Application/"]
RUN dotnet restore "CardsApp.Api/CardsApp.Api.csproj"
COPY . .
WORKDIR "/src/CardsApp.Api"
RUN dotnet build "CardsApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CardsApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CardsApp.Api.dll"]
