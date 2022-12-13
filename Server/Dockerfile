#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Server/totten_romatoes.Server.csproj", "Server/"]
COPY ["Client/totten_romatoes.Client.csproj", "Client/"]
COPY ["Shared/totten_romatoes.Shared.csproj", "Shared/"]
RUN dotnet restore "Server/totten_romatoes.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "totten_romatoes.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "totten_romatoes.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "totten_romatoes.Server.dll"]