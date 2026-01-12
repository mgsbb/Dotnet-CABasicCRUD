FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /myapp

ARG PUBLISH_PROJECT_PATH
ARG PERSISTENCE_PROJECT_PATH
ARG ConnectionStrings__DefaultConnection

RUN dotnet tool install --global dotnet-ef --version 9.*
ENV PATH="$PATH:/root/.dotnet/tools"

COPY CABasicCRUD.sln .

# copy all projects csproj files (less than ideal?)
COPY ./src/CABasicCRUD.Application/CABasicCRUD.Application.csproj ./src/CABasicCRUD.Application/CABasicCRUD.Application.csproj
COPY ./src/CABasicCRUD.Domain/CABasicCRUD.Domain.csproj ./src/CABasicCRUD.Domain/CABasicCRUD.Domain.csproj
COPY ./src/CABasicCRUD.Host.PostgreSql.WebApi/CABasicCRUD.Host.PostgreSql.WebApi.csproj ./src/CABasicCRUD.Host.PostgreSql.WebApi/CABasicCRUD.Host.PostgreSql.WebApi.csproj
COPY ./src/CABasicCRUD.Host.Sqlite.WebApi/CABasicCRUD.Host.Sqlite.WebApi.csproj ./src/CABasicCRUD.Host.Sqlite.WebApi/CABasicCRUD.Host.Sqlite.WebApi.csproj
COPY ./src/CABasicCRUD.Infrastructure/CABasicCRUD.Infrastructure.csproj ./src/CABasicCRUD.Infrastructure/CABasicCRUD.Infrastructure.csproj
COPY ./src/CABasicCRUD.Infrastructure.Persistence.PostgreSql/CABasicCRUD.Infrastructure.Persistence.PostgreSql.csproj ./src/CABasicCRUD.Infrastructure.Persistence.PostgreSql/CABasicCRUD.Infrastructure.Persistence.PostgreSql.csproj
COPY ./src/CABasicCRUD.Infrastructure.Persistence.Sqlite/CABasicCRUD.Infrastructure.Persistence.Sqlite.csproj ./src/CABasicCRUD.Infrastructure.Persistence.Sqlite/CABasicCRUD.Infrastructure.Persistence.Sqlite.csproj
COPY ./src/CABasicCRUD.Presentation.WebApi/CABasicCRUD.Presentation.WebApi.csproj ./src/CABasicCRUD.Presentation.WebApi/CABasicCRUD.Presentation.WebApi.csproj

# even the test projects
COPY ./tests/CABasicCRUD.UnitTests.Application/CABasicCRUD.UnitTests.Application.csproj ./tests/CABasicCRUD.UnitTests.Application/CABasicCRUD.UnitTests.Application.csproj
COPY ./tests/CABasicCRUD.UnitTests.Domain/CABasicCRUD.UnitTests.Domain.csproj ./tests/CABasicCRUD.UnitTests.Domain/CABasicCRUD.UnitTests.Domain.csproj

RUN dotnet restore

COPY ./src/ ./src/

# slow
RUN dotnet ef migrations bundle \
    --project ${PERSISTENCE_PROJECT_PATH} \
    --startup-project ${PUBLISH_PROJECT_PATH} \
    --self-contained \
    -r linux-x64

# what if a different composition root is to be published? solution: use build time ARG
RUN dotnet publish ${PUBLISH_PROJECT_PATH} \
    -c Release \
    -o ./publish \
    --no-restore


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /myapp

ARG PUBLISHED_PROJECT_ENTRYPOINT

COPY --from=build /myapp/publish ./publish
COPY --from=build /myapp/efbundle ./publish/
EXPOSE 5002

# JSONArgsRecommended: JSON arguments recommended for ENTRYPOINT to prevent unintended behavior related to OS signals
# ENTRYPOINT [ "dotnet", ${PUBLISHED_PROJECT_ENTRYPOINT} ]

# without setting ENV, the value will not be available inside the container
ENV PUBLISHED_PROJECT_ENTRYPOINT=${PUBLISHED_PROJECT_ENTRYPOINT}
ENTRYPOINT dotnet ${PUBLISHED_PROJECT_ENTRYPOINT}

# ENTRYPOINT [ "sleep", "infinity" ]