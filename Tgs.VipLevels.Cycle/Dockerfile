#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.VipLevels.Cycle/Tgs.VipLevels.Cycle.csproj", "Tgs.VipLevels.Cycle/"]
COPY ["Tgs.VipLevels.Cycle.Core/Tgs.VipLevels.Cycle.Core.csproj", "Tgs.VipLevels.Cycle.Core/"]
COPY ["Tgs.VipLevels.Cycle.Common/Tgs.VipLevels.Cycle.Common.csproj", "Tgs.VipLevels.Cycle.Common/"]
COPY ["Tgs.VipLevels.Cycle.Repositories/Tgs.VipLevels.Cycle.Repositories.csproj", "Tgs.VipLevels.Cycle.Repositories/"]
RUN dotnet restore "./Tgs.VipLevels.Cycle/./Tgs.VipLevels.Cycle.csproj"
COPY . .
WORKDIR "/src/Tgs.VipLevels.Cycle"
RUN dotnet build "./Tgs.VipLevels.Cycle.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.VipLevels.Cycle.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.VipLevels.Cycle.dll"]