using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace twitchDnd.Client.Services.Communication;

public class SignalRHub : IDisposable
{
	private readonly ILocalStorageService _localStorageService;
	private readonly NavigationManager _navigationManager;
	public  HubConnection Connection { get; private set; }

	public SignalRHub(NavigationManager navigationManager, ILocalStorageService localStorageService)
	{
		_navigationManager = navigationManager;
		_localStorageService = localStorageService;
	}

	public Task Connect()
	{
		Connection = new HubConnectionBuilder().WithUrl(_navigationManager.ToAbsoluteUri("/hub"), options =>
		{
			options.AccessTokenProvider = async () => await _localStorageService.GetItemAsync<string>("authToken");
		}).Build();

		return Connection.StartAsync();
	}

	public void Dispose()
	{
		Connection?.DisposeAsync().ConfigureAwait(false);
	}
}