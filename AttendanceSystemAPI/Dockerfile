FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AttendanceSystemAPI/AttendanceSystemAPI.csproj", "AttendanceSystemAPI/"]
RUN dotnet restore "AttendanceSystemAPI/AttendanceSystemAPI.csproj"
COPY . .
WORKDIR "/src/AttendanceSystemAPI"
RUN dotnet build "AttendanceSystemAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AttendanceSystemAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AttendanceSystemAPI.dll"]