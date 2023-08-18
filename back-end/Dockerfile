FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /src
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore

RUN dotnet build "back-end.csproj" -c Release -o /app/build

# Build and publish a release
FROM build AS publish
#RUN dotnet publish -c Release -o out
RUN dotnet publish "back-end.csproj" -c Release -o /app/publish


# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "back-end.dll"]