#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BevCapital.StockPrices.API/BevCapital.StockPrices.API.csproj", "BevCapital.StockPrices.API/"]
COPY ["BevCapital.StockPrices.Application/BevCapital.StockPrices.Application.csproj", "BevCapital.StockPrices.Application/"]
COPY ["BevCapital.StockPrices.Domain/BevCapital.StockPrices.Domain.csproj", "BevCapital.StockPrices.Domain/"]
COPY ["BevCapital.StockPrices.Data/BevCapital.StockPrices.Data.csproj", "BevCapital.StockPrices.Data/"]
COPY ["BevCapital.StockPrices.Infra/BevCapital.StockPrices.Infra.csproj", "BevCapital.StockPrices.Infra/"]


RUN dotnet restore "BevCapital.StockPrices.API/BevCapital.StockPrices.API.csproj"
COPY . .
WORKDIR "/src/BevCapital.StockPrices.API"
RUN dotnet build "BevCapital.StockPrices.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BevCapital.StockPrices.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BevCapital.StockPrices.API.dll"]