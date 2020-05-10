FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["PublishService/PublishService.csproj", "PublishService/"]

RUN dotnet restore "PublishService/PublishService.csproj"
COPY . .
WORKDIR "/src/PublishService"
RUN dotnet build "PublishService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PublishService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublishService.dll"]