FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/NotificationService.WebAPI/NotificationService.WebAPI.csproj", "src/NotificationService.WebAPI/"]
RUN dotnet restore "src/NotificationService.WebAPI/NotificationService.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/NotificationService.WebAPI"
RUN dotnet build "NotificationService.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationService.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.WebAPI.dll"]
