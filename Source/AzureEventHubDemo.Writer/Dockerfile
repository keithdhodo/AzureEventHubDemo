#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Source/AzureEventHubDemo.Writer/AzureEventHubDemo.Writer.csproj", "Source/AzureEventHubDemo.Writer/"]
RUN dotnet restore "Source/AzureEventHubDemo.Writer/AzureEventHubDemo.Writer.csproj"
COPY . .
WORKDIR "/src/Source/AzureEventHubDemo.Writer"
RUN dotnet build "AzureEventHubDemo.Writer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AzureEventHubDemo.Writer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AzureEventHubDemo.Writer.dll"]