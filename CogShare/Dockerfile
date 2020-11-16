#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["CogShare/CogShare.csproj", "CogShare/"]
RUN dotnet restore "CogShare/CogShare.csproj"
COPY . .
WORKDIR "/src/CogShare"
RUN dotnet build "CogShare.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CogShare.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CogShare.dll"]