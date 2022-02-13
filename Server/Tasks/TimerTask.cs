using System;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.SignalR;
using twitchDnd.Server.Hubs;
using twitchDnd.Shared.Models.Hubs;
using twitchDnd.Shared.Models.Timer;

namespace twitchDnd.Server.Tasks;

// ReSharper disable once UnusedType.Global
public class TimerTask : ITask
{
	private readonly IHubContext<ServerSignalRHub> _hub;
	private readonly Timer _timer;
	private readonly TimerModal _modal;

	public TimerTask(IHubContext<ServerSignalRHub> hub)
	{
		_hub = hub;
		_modal = new TimerModal(new TimeSpan(1, 0, 0));
		_timer = new Timer(1000)
		{
			AutoReset = true,
			Enabled = true,
		};
		_timer.Elapsed += (sender, args) =>
		{
			_modal.AddSecond();
			_hub.Clients?.All?.SendCoreAsync(HubMethods.ReceiveMessage, new[] {_modal});
		};
	}
	public Task Start()
	{
		_timer.Start();
		return Task.CompletedTask;
	}

	public bool Completed => false; // long running task
}