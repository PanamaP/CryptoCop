FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

LABEL maintainer="Elfar Snær Arnarson"
LABEL author="Elfar Snær Arnarson"

WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -o /app/published-app

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "Cryptocop.Software.API.dll", "--server.urls", "https://+:5000" ]