FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AutoMotiveProject.cs.csproj", "./"]
RUN dotnet restore "AutoMotiveProject.cs.csproj"
COPY . .
RUN dotnet build "AutoMotiveProject.cs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AutoMotiveProject.cs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY service-account.json .
COPY credentials.txt .
ENTRYPOINT ["dotnet", "AutoMotiveProject.cs.dll"]