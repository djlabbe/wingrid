FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet-build
WORKDIR /src
COPY . /src
RUN dotnet restore
RUN dotnet build "Source/Web/Web.csproj" --configuration Release

FROM dotnet-build AS dotnet-publish
RUN dotnet publish "Source/Web/Web.csproj" -c Release -o /app/publish

FROM node AS node-builder
WORKDIR /node
COPY ./Source/Web/ClientApp /node
RUN npm install
RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 3000
RUN mkdir /app/wwwroot
COPY --from=dotnet-publish /app/publish .
COPY --from=node-builder /node/dist ./wwwroot
ENTRYPOINT ["dotnet", "Web.dll"]