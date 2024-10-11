FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app
EXPOSE 80 443 8000

COPY *.csproj ./

RUN dotnet restore

COPY . ./

RUN ls -R /app
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final-env

WORKDIR /app

COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS=http://+:8000

ENTRYPOINT ["dotnet", "Rezervacije.Web.dll"]