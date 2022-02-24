using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using twitchDnd.Client.Services.Communication;
using twitchDnd.Client.Services.Web;
using twitchDnd.Shared.Bases;
using twitchDnd.Shared.Models.Hubs;
using twitchDnd.Shared.Models.Timer;

namespace twitchDnd.Client.ViewModels.Timer
{
	public class TimerViewModel : BaseNotifyStateChanged
	{
		private readonly SignalRHub _signalRHub;
		public TimerSession Session { get; private set; } = new();
		private AuthedHttpClient _httpClient;
		public bool IsLoading { get; private set; } = true;
		public string TimerValue => Session.RemainingTimeForCurrentMode.ToString(@"mm\:ss");

		public string ModeDisplay
		{
			get
			{
				return Session.Mode switch
				{
					TimerMode.CollectingResponses => "Collecting Responses",
					TimerMode.Voting => "Voting",
					TimerMode.Finished => "Finished",
					_ => throw new ArgumentOutOfRangeException()
				};
			}
		}

		public TimerViewModel(SignalRHub signalRHub, AuthedHttpClient httpClient)
		{
			_signalRHub = signalRHub;
			_httpClient = httpClient;
			SetupConnection().ConfigureAwait(false);
		}

		private async Task SetupConnection()
		{
			await _signalRHub.Connect();
			await _httpClient.Init();
			var response = await _httpClient.GetFromJsonAsync<TimerSession>("timer");
			if (response != null)
			{
				Session = response;
				IsLoading = false;
				NotifyStateChanged();
			}
			_signalRHub.Connection.On<TimerSession>(HubMethods.TimerSession, session =>
			{
				Session = session; NotifyStateChanged();
			});
		}

		public bool Editing => !Session.Running;

		public Task Start()
		{
			Session.Mode = TimerMode.CollectingResponses;
			return _httpClient.PostAsJsonAsync("timer/start", Session);
		}

		public Task Stop()
		{
			return _httpClient.PostAsync("timer/stop", null);
		}
	}
}