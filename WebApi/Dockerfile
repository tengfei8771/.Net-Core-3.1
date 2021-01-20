#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["WebApi.csproj", ""]
COPY ["../Models/Models.csproj", "../Models/"]
COPY ["../Utils/Utils.csproj", "../Utils/"]
COPY ["../IServices/IServices.csproj", "../IServices/"]
COPY ["../IRepository/IRepository.csproj", "../IRepository/"]
COPY ["../Entity/Entity.csproj", "../Entity/"]
COPY ["../Repository/Repository.csproj", "../Repository/"]
COPY ["../Services/Services.csproj", "../Services/"]
COPY ["../ConsulBuilder/ConsulBuilder.csproj", "../ConsulBuilder/"]
COPY ["../Redis/Redis.csproj", "../Redis/"]
COPY ["../Dispatch/Dispatch.csproj", "../Dispatch/"]
RUN dotnet restore "./WebApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]