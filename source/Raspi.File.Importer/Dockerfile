#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["source/Raspi.File.Importer/Raspi.File.Importer.csproj", "source/Raspi.File.Importer/"]
RUN dotnet restore "source/Raspi.File.Importer/Raspi.File.Importer.csproj"
COPY . .
WORKDIR "/src/source/Raspi.File.Importer"
RUN dotnet build "Raspi.File.Importer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Raspi.File.Importer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Raspi.File.Importer.dll"]