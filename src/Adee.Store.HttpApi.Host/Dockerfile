#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM adeewu-docker.pkg.coding.net/adee.store/aspnet/runtime:5.0-alpine
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM adeewu-docker.pkg.coding.net/adee.store/aspnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/Adee.Store.HttpApi.Host/Adee.Store.HttpApi.Host.csproj", "src/Adee.Store.HttpApi.Host/"]
COPY ["src/Adee.Store.HttpApi/Adee.Store.HttpApi.csproj", "src/Adee.Store.HttpApi/"]
COPY ["src/Adee.Store.Application.Contracts/Adee.Store.Application.Contracts.csproj", "src/Adee.Store.Application.Contracts/"]
COPY ["src/Adee.Store.Domain.Shared/Adee.Store.Domain.Shared.csproj", "src/Adee.Store.Domain.Shared/"]
COPY ["src/Adee.Store.Application/Adee.Store.Application.csproj", "src/Adee.Store.Application/"]
COPY ["src/Adee.Store.Domain/Adee.Store.Domain.csproj", "src/Adee.Store.Domain/"]
COPY ["src/Adee.Store.EntityFrameworkCore.DbMigrations/Adee.Store.EntityFrameworkCore.DbMigrations.csproj", "src/Adee.Store.EntityFrameworkCore.DbMigrations/"]
COPY ["src/Adee.Store.EntityFrameworkCore/Adee.Store.EntityFrameworkCore.csproj", "src/Adee.Store.EntityFrameworkCore/"]
RUN dotnet restore "src/Adee.Store.HttpApi.Host/Adee.Store.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/Adee.Store.HttpApi.Host"
RUN dotnet build "Adee.Store.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adee.Store.HttpApi.Host.csproj" -c Release -o /app/publish

FROM adeewu-docker.pkg.coding.net/adee.store/aspnet/runtime:5.0-alpine
WORKDIR /app
COPY --from=2 /app/publish .
ENTRYPOINT ["dotnet", "Adee.Store.HttpApi.Host.dll"]