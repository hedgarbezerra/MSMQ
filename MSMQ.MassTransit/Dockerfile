#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MSMQ.MassTransit/MSMQ.MassTransit.csproj", "MSMQ.MassTransit/"]
RUN dotnet restore "./MSMQ.MassTransit/MSMQ.MassTransit.csproj"
COPY . .
WORKDIR "/src/MSMQ.MassTransit"
RUN dotnet build "./MSMQ.MassTransit.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MSMQ.MassTransit.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MSMQ.MassTransit.dll"]