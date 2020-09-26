FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY ExpenseTracker.csproj /build/

RUN dotnet restore ./build/ExpenseTracker.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./ExpenseTracker.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "ExpenseTracker.dll"] 