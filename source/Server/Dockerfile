#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["source/Server/Raspi.Temperature.App.Server.csproj", "source/Server/"]
COPY ["source/Shared/Raspi.Temperature.App.Shared.csproj", "source/Shared/"]
COPY ["source/Client/Raspi.Temperature.App.Client.csproj", "source/Client/"]
RUN dotnet restore "source/Server/Raspi.Temperature.App.Server.csproj"
COPY . .
WORKDIR "/src/source/Server"
RUN dotnet build "Raspi.Temperature.App.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Raspi.Temperature.App.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Raspi.Temperature.App.Server.dll"]