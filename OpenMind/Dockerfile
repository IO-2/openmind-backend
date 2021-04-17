FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OpenMind/OpenMind.csproj", "OpenMind/"]
RUN dotnet restore "OpenMind/OpenMind.csproj"
COPY . .
WORKDIR "/src/OpenMind"
RUN dotnet build "OpenMind.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenMind.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenMind.dll"]
