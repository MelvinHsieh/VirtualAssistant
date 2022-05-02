#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./dataservice/dataservice.csproj", "."]
COPY ["./dataservice/*.sln", "."]

COPY ["./Application", "."]
COPY ["./Domain", "."]
COPY ["./Infrastructure", "."]
COPY ["./dataservice.Tests", "."]

RUN dotnet restore "./dataservice.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "dataservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dataservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dataservice.dll"]