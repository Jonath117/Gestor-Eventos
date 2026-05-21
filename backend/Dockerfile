FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS development
WORKDIR /app
EXPOSE 5206
CMD ["dotnet", "watch", "run", "--project", "src/Web.API/Web.API.csproj", "--urls", "http://+:5206"]

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "Directory.Build.props"]
COPY ["Directory.Packages.props", "Directory.Packages.props"]

COPY ["src/Modules/Core/Core.Domain/Core.Domain.csproj", "Modules/Core/Core.Domain/"]
COPY ["src/Modules/Core/Core.Application/Core.Application.csproj", "Modules/Core/Core.Application/"]
COPY ["src/Modules/Core/Core.Infrastructure/Core.Infrastructure.csproj", "Modules/Core/Core.Infrastructure/"]
COPY ["src/Modules/Core/Core.Presentation/Core.Presentation.csproj", "Modules/Core/Core.Presentation/"]


COPY ["src/Modules/Identity/Identity.Domain/Identity.Domain.csproj", "Modules/Identity/Identity.Domain/"]
COPY ["src/Modules/Identity/Identity.Application/Identity.Application.csproj", "Modules/Identity/Identity.Application/"]
COPY ["src/Modules/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "Modules/Identity/Identity.Infrastructure/"]
COPY ["src/Modules/Identity/Identity.Presentation/Identity.Presentation.csproj", "Modules/Identity/Identity.Presentation/"]

COPY ["src/Modules/Logistics/Logistics.Domain/Logistics.Domain.csproj", "Modules/Logistics/Logistics.Domain/"]
COPY ["src/Modules/Logistics/Logistics.Application/Logistics.Application.csproj", "Modules/Logistics/Logistics.Application/"]
COPY ["src/Modules/Logistics/Logistics.Infrastructure/Logistics.Infrastructure.csproj", "Modules/Logistics/Logistics.Infrastructure/"]

COPY ["src/Modules/Registration/Registration.Domain/Registration.Domain.csproj", "Modules/Registration/Registration.Domain/"]
COPY ["src/Modules/Registration/Registration.Application/Registration.Application.csproj", "Modules/Registration/Registration.Application/"]
COPY ["src/Modules/Registration/Registration.Infrastructure/Registration.Infrastructure.csproj", "Modules/Registration/Registration.Infrastructure/"]

COPY ["src/Modules/Payment/Payment.Domain/Payment.Domain.csproj", "Modules/Payment/Payment.Domain/"]
COPY ["src/Modules/Payment/Payment.Application/Payment.Application.csproj", "Modules/Payment/Payment.Application/"]
COPY ["src/Modules/Payment/Payment.Infrastructure/Payment.Infrastructure.csproj", "Modules/Payment/Payment.Infrastructure/"]
COPY ["src/Modules/Payment/Payment.Presentation/Payment.Presentation.csproj", "Modules/Payment/Payment.Presentation/"]

COPY ["src/Web.API/Web.API.csproj", "Web.API/"]

RUN dotnet restore "Web.API/Web.API.csproj"
COPY src .
RUN dotnet publish "Web.API/Web.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

USER $APP_UID

ENTRYPOINT ["dotnet", "Web.API.dll"]