#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SalveApp.Clinics.Web/SalveApp.Clinics.Web.csproj", "SalveApp.Clinics.Web/"]
COPY ["SalveApp.Clinics.Core/SalveApp.Clinics.Core.csproj", "SalveApp.Clinics.Core/"]
RUN dotnet restore "SalveApp.Clinics.Web/SalveApp.Clinics.Web.csproj"
COPY . .
WORKDIR "/src/SalveApp.Clinics.Web"
RUN dotnet build "SalveApp.Clinics.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SalveApp.Clinics.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalveApp.Clinics.Web.dll"]