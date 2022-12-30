#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 7000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["DataService.HostApi/DataService.HostApi.csproj", "DataService.HostApi/"]
COPY ["DataService.Swagger/DataService.Swagger.csproj", "DataService.Swagger/"]
COPY ["DataService.Shared/DataService.Shared.csproj", "DataService.Shared/"]
COPY ["DataService.Dto/DataService.Dto.csproj", "DataService.Dto/"]
COPY ["DataService.Application/DataService.Application.csproj", "DataService.Application/"]
COPY ["DataService.Application.Contracts/DataService.Application.Contracts.csproj", "DataService.Application.Contracts/"]
COPY ["DataService.Domain/DataService.Domain.csproj", "DataService.Domain/"]
COPY ["DataService.SqlSugarOrm/DataService.SqlSugarOrm.csproj", "DataService.SqlSugarOrm/"]
COPY ["DataService.JWT/DataService.JWT.csproj", "DataService.JWT/"]
COPY ["DataService.Redis/DataService.Redis.csproj", "DataService.Redis/"]
COPY ["DataService.Mongo/DataService.Mongo.csproj", "DataService.Mongo/"]
RUN dotnet restore "DataService.HostApi/DataService.HostApi.csproj"
COPY . .
WORKDIR "/src/DataService.HostApi"
RUN dotnet build "DataService.HostApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataService.HostApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataService.HostApi.dll"]