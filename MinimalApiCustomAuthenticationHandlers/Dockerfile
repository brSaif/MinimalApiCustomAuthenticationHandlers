FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MinimalApiCustomAuthenticationHandlers/MinimalApiCustomAuthenticationHandlers.csproj", "MinimalApiCustomAuthenticationHandlers/"]
RUN dotnet restore "MinimalApiCustomAuthenticationHandlers/MinimalApiCustomAuthenticationHandlers.csproj"
COPY . .
WORKDIR "/src/MinimalApiCustomAuthenticationHandlers"
RUN dotnet build "MinimalApiCustomAuthenticationHandlers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinimalApiCustomAuthenticationHandlers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalApiCustomAuthenticationHandlers.dll"]
