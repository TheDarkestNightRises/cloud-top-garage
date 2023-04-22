FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CarService.csproj", "./"]
RUN dotnet restore "CarService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CarService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["Images/car.png", "/app/Images/"]
ENTRYPOINT ["dotnet", "CarService.dll"]
