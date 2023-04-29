# Base phase. Pull the base image from microsoft ASP.NET 
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base  
# Change the working directory to /app
WORKDIR /app  
# Expose port 80 for communication 
EXPOSE 80

# Building phase
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build  
# Change the working directory to /src
WORKDIR /src  
# Copy the csproj file that contains the project configuration
COPY ["CarService.csproj", "./"]  
# Restore dependecies
RUN dotnet restore "CarService.csproj" 
# Copy the entire project
COPY . .
# Change the working directory to /src
WORKDIR "/src/."  
# Build the project and release the output in /app/build
RUN dotnet build "CarService.csproj" -c Release -o /app/build  

#Publishing phase 
FROM build AS publish 
# Publish the app and release the output to /app/publish,
RUN dotnet publish "CarService.csproj" -c Release -o /app/publish /p:UseAppHost=false  

# Final phase.
FROM base AS final 
# Change the working directory to /app
WORKDIR /app  
# Copy the files from the publishing phase
COPY --from=publish /app/publish .  
# Copy the images 
COPY ["Images/car.png", "/app/Images/"] 
# Set the starting point for the container.
ENTRYPOINT ["dotnet", "CarService.dll"]  
