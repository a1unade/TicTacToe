FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TicTacToe.WebAPI/TicTacToe.WebAPI.csproj", "TicTacToe.WebAPI/"]
COPY ["TicTacToe.Application/TicTacToe.Application.csproj", "TicTacToe.Application/"]
COPY ["TicTacToe.Domain/TicTacToe.Domain.csproj", "TicTacToe.Domain/"]
COPY ["TicTacToe.Infrastructure/TicTacToe.Infrastructure.csproj", "TicTacToe.Infrastructure/"]
COPY ["TicTacToe.Persistence/TicTacToe.Persistence.csproj", "TicTacToe.Persistence/"]
RUN dotnet restore "TicTacToe.WebAPI/TicTacToe.WebAPI.csproj"
COPY . .
WORKDIR "/src/TicTacToe.WebAPI"
RUN dotnet build "TicTacToe.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TicTacToe.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicTacToe.WebAPI.dll"]
