FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /myapp

ARG ConnectionStrings__DefaultConnection

RUN dotnet tool install --global dotnet-ef --version 9.*
ENV PATH="$PATH:/root/.dotnet/tools"

COPY CABasicCRUD.sln .

COPY ./src/CABasicCRUD.Application/CABasicCRUD.Application.csproj ./src/CABasicCRUD.Application/CABasicCRUD.Application.csproj
COPY ./src/CABasicCRUD.Domain/CABasicCRUD.Domain.csproj ./src/CABasicCRUD.Domain/CABasicCRUD.Domain.csproj
COPY ./src/CABasicCRUD.Host.PostgreSql.WebApi/CABasicCRUD.Host.PostgreSql.WebApi.csproj ./src/CABasicCRUD.Host.PostgreSql.WebApi/CABasicCRUD.Host.PostgreSql.WebApi.csproj
COPY ./src/CABasicCRUD.Infrastructure/CABasicCRUD.Infrastructure.csproj ./src/CABasicCRUD.Infrastructure/CABasicCRUD.Infrastructure.csproj
COPY ./src/CABasicCRUD.Infrastructure.Persistence.PostgreSql/CABasicCRUD.Infrastructure.Persistence.PostgreSql.csproj ./src/CABasicCRUD.Infrastructure.Persistence.PostgreSql/CABasicCRUD.Infrastructure.Persistence.PostgreSql.csproj
COPY ./src/CABasicCRUD.Presentation.WebApi/CABasicCRUD.Presentation.WebApi.csproj ./src/CABasicCRUD.Presentation.WebApi/CABasicCRUD.Presentation.WebApi.csproj

COPY ./src/ ./src/

RUN dotnet ef migrations bundle \
    --project ./src/CABasicCRUD.Infrastructure.Persistence.PostgreSql \
    --startup-project ./src/CABasicCRUD.Host.PostgreSql.WebApi \
    --self-contained \
    -r linux-x64

RUN dotnet publish ./src/CABasicCRUD.Host.PostgreSql.WebApi \
    -c Release \
    -o ./publish \
    --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /myapp


COPY --from=build /myapp/publish ./publish
COPY --from=build /myapp/efbundle ./publish/
EXPOSE 5002

WORKDIR /myapp/publish

ENTRYPOINT ["dotnet", "CABasicCRUD.Host.PostgreSql.WebApi.dll"]