FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CurrencyWebSite/CurrencyWebSite.csproj", "CurrencyWebSite/"]
RUN dotnet restore "CurrencyWebSite/CurrencyWebSite.csproj"
COPY . .
WORKDIR "/src/CurrencyWebSite"
RUN dotnet build "CurrencyWebSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurrencyWebSite.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyWebSite.dll"]

