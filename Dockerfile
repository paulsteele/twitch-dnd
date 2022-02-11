FROM mcr.microsoft.com/dotnet/sdk:6.0 as builder
WORKDIR /twitchDnd

COPY twitch-dnd.sln .
COPY Client/twitchDnd.Client.csproj Client/twitchDnd.Client.csproj
COPY Server/twitchDnd.Server.csproj Server/twitchDnd.Server.csproj
COPY Shared/twitchDnd.Shared.csproj Shared/twitchDnd.Shared.csproj
COPY .nuke .nuke
COPY Build Build
COPY build.sh .

RUN ./build.sh restore

COPY . .

RUN ./build.sh publish 

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runner
WORKDIR /twitchDnd

COPY --from=builder /twitchDnd/Client/bin/Release/net5.0/publish ./Client
COPY --from=builder /twitchDnd/Server/bin/Release/net5.0/publish ./Server

WORKDIR /twitchDnd/Client

ENTRYPOINT [ "dotnet", "/twitchDnd/Server/twitchDnd.Server.dll"]
