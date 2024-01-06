#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SkillsHub/SkillsHub.csproj", "SkillsHub/"]
COPY ["EmailProvider/EmailProvider.csproj", "EmailProvider/"]
COPY ["SkillsDomain/SkillsHub.Domain.csproj", "SkillsDomain/"]
COPY ["SkillsHub.Application/SkillsHub.Application.csproj", "SkillsHub.Application/"]
COPY ["SkillsHub.Persistence/SkillsHub.Persistence.csproj", "SkillsHub.Persistence/"]
COPY ["ViberMailer/ViberMailer.csproj", "ViberMailer/"]
RUN dotnet restore "./SkillsHub/./SkillsHub.csproj"
COPY . .
WORKDIR "/src/SkillsHub"
RUN dotnet build "./SkillsHub.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SkillsHub.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SkillsHub.dll"]