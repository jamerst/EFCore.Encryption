FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app

# Copy everything else and build
ADD ./EFCore.Encryption.Tests ./EFCore.Encryption.Tests
ADD ./EFCore.Encryption ./EFCore.Encryption

WORKDIR /app/EFCore.Encryption.Tests

RUN dotnet restore
RUN dotnet publish -c Release -o out

ENTRYPOINT ["dotnet", "test"]