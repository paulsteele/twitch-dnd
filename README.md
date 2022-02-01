# twitch-dnd
Webpage to facilitate twitch plays dnd

# Dependencies
* [Dotnet 5](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
* [Podman](https://podman.io/getting-started/)
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)

# Development
This project is created using [Blazor](https://dotnet.microsoft.com/learn/aspnet/blazor-tutorial/install)

It utilizes an asp.net backend and a WASM frontend with shared models between each side
 ## Running Database Locally
* Ensure podman is running
* first time
  * run `./tools/create-containers.sh`
* other times
  * run `./tools/start-containers.sh`
* stopping
  * run `./tools/stop-containers.sh`

## Errors
```      
System.InvalidOperationException: An exception has been raised that is likely due to a transient failure. Consider enabling transient error resiliency by adding 'EnableRetryOnFailure()' to the 'UseMySql' call.
       ---> MySql.Data.MySqlClient.MySqlException (0x80004005): Unable to connect to any of the specified MySQL hosts.
```
* The database is not running / reachable on default port
