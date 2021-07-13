#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM adeewu-docker.pkg.coding.net/adee.store/aspnet/runtime:5.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM adeewu-docker.pkg.coding.net/adee.store/aspnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/Adee.Store.IdentityServer/Adee.Store.IdentityServer.csproj", "src/Adee.Store.IdentityServer/"]
COPY ["src/Adee.Store.EntityFrameworkCore.DbMigrations/Adee.Store.EntityFrameworkCore.DbMigrations.csproj", "src/Adee.Store.EntityFrameworkCore.DbMigrations/"]
COPY ["src/Adee.Store.EntityFrameworkCore/Adee.Store.EntityFrameworkCore.csproj", "src/Adee.Store.EntityFrameworkCore/"]
COPY ["src/Adee.Store.Domain/Adee.Store.Domain.csproj", "src/Adee.Store.Domain/"]
COPY ["src/Adee.Store.Domain.Shared/Adee.Store.Domain.Shared.csproj", "src/Adee.Store.Domain.Shared/"]
RUN dotnet restore "src/Adee.Store.IdentityServer/Adee.Store.IdentityServer.csproj"
COPY . .
WORKDIR "/src/src/Adee.Store.IdentityServer"
RUN dotnet build "Adee.Store.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adee.Store.IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adee.Store.IdentityServer.dll"]