FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY ExpenseTracker.AuthApi.csproj /build/

RUN dotnet restore ./build/ExpenseTracker.AuthApi.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./ExpenseTracker.AuthApi.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "ExpenseTracker.AuthApi.dll"] 