FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ProductsMinimalAPI/ProductsMinimalAPI.csproj", "ProductsMinimalAPI/"]
RUN dotnet restore "ProductsMinimalAPI/ProductsMinimalAPI.csproj"
COPY . .
WORKDIR "/src/ProductsMinimalAPI"
RUN dotnet build "ProductsMinimalAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductsMinimalAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductsMinimalAPI.dll"]
