#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY **/*.csproj ./
RUN for project in *.csproj; \
do \
dotnet restore "$project"; \

COPY . .
WORKDIR "/src/Bcss.Wallboard.Api"
RUN dotnet build "Bcss.Wallboard.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bcss.Wallboard.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bcss.Wallboard.Api.dll"]