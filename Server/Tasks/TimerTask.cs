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
	private Timer _timer;

	public TimerTask(IHubContext<ServerSignalRHub> hub)
	{
		_hub = hub;
		Session = new TimerSession();
	}

	public TimerSession Session { get; set; }

	public Task Start()
	{
		return Task.CompletedTask;
	}

	public bool Completed => false; // long running task

	public void StartTimer()
	{
		lock (this)
		{
			_timer?.Dispose();
			_timer = new Timer(1000)
			{
				AutoReset = true,
				Enabled = true
			};
			_timer.Elapsed += (sender, args) =>
			{
				var running = Session.AddSecond();
				_hub.Clients?.All?.SendCoreAsync(HubMethods.TimerSession, new[] {Session});
				if (!running)
				{
					StopTimer();
				}
			};
			_timer.Start();
			Session.Start();
			_hub.Clients?.All?.SendCoreAsync(HubMethods.TimerSession, new[] {Session});
		}
	}

	public void StopTimer()
	{
		lock (this)
		{
			Session.Pause();
			_timer.Stop();
			_hub.Clients?.All?.SendCoreAsync(HubMethods.TimerSession, new[] {Session});
		}
	}
}