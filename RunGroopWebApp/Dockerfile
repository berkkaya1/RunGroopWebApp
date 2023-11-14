FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RunGroopWebApp/RunGroopWebApp.csproj", "RunGroopWebApp/"]
RUN dotnet restore "RunGroopWebApp/RunGroopWebApp.csproj"
COPY . .
WORKDIR "/src/RunGroopWebApp"
RUN dotnet build "RunGroopWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RunGroopWebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RunGroopWebApp.dll"]
