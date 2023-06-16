FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OrdersMinimalAPI/OrdersMinimalAPI.csproj", "OrdersMinimalAPI/"]
RUN dotnet restore "OrdersMinimalAPI/OrdersMinimalAPI.csproj"
COPY . .
WORKDIR "/src/OrdersMinimalAPI"
RUN dotnet build "OrdersMinimalAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrdersMinimalAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrdersMinimalAPI.dll"]
