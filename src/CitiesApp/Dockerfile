#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CitiesApp/CitiesApp.csproj", "CitiesApp/"]
COPY ["CitiesApp.Application/CitiesApp.Application.csproj", "CitiesApp.Application/"]
COPY ["CitiesApp.Domain/CitiesApp.Domain.csproj", "CitiesApp.Domain/"]
COPY ["CitiesApp.Infrastructure/CitiesApp.Infrastructure.csproj", "CitiesApp.Infrastructure/"]
COPY ["CitiesApp.Presentation/CitiesApp.Presentation.csproj", "CitiesApp.Presentation/"]
RUN dotnet restore "CitiesApp/CitiesApp.csproj"
COPY . .
WORKDIR "/src/CitiesApp"
RUN dotnet build "CitiesApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CitiesApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CitiesApp.dll"]