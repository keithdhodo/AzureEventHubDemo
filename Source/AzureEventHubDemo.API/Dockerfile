#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Source/AzureEventHubDemo.API/AzureEventHubDemo.API.csproj", "Source/AzureEventHubDemo.API/"]
RUN dotnet restore "Source/AzureEventHubDemo.API/AzureEventHubDemo.API.csproj"
COPY . .
WORKDIR "/src/Source/AzureEventHubDemo.API"
RUN dotnet build "AzureEventHubDemo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AzureEventHubDemo.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureEventHubDemo.API.dll"]