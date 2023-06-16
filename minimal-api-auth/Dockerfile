FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MinimalAPIJWTAuth.csproj", "./"]
RUN dotnet restore "MinimalAPIJWTAuth.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "MinimalAPIJWTAuth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinimalAPIJWTAuth.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalAPIJWTAuth.dll"]
