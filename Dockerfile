FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 60

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Entregable-Universities/Entregable-Universities.csproj", "Entregable-Universities/"]
RUN dotnet restore "Entregable-Universities/Entregable-Universities.csproj"

COPY . .
WORKDIR "/src/Entregable-Universities"
RUN dotnet build "Entregable-Universities.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Entregable-Universities.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Entregable-Universities.dll"]
