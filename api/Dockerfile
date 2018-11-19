FROM microsoft/dotnet:2.1-sdk
ARG PROJECT_DIR=/srv/api
RUN mkdir -p $PROJECT_DIR
WORKDIR $PROJECT_DIR
COPY ./ ./
RUN dotnet build
