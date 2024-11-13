FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Contacts-API.sln ./
COPY Contacts-API.Application/Contacts-API.Application.csproj Contacts-API.Application/
COPY Contacts-API.Domain/Contacts-API.Domain.csproj Contacts-API.Domain/
COPY Contacts-API.Infrastructure/Contacts-API.Infrastructure.csproj Contacts-API.Infrastructure/
COPY Contacts-API.Tests/Contacts-API.Tests.csproj Contacts-API.Tests/
COPY Contacts-API/Contacts-API.API.csproj Contacts-API/
RUN dotnet restore

COPY . ./
RUN dotnet publish Contacts-API/Contacts-API.API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5000

ENTRYPOINT ["dotnet", "Contacts-API.API.dll"]
