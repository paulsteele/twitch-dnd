using System.Timers;
using Microsoft.AspNetCore.SignalR;
using twitchDnd.Server.Hubs;
using twitchDnd.Shared.Models.Hubs;
using twitchDnd.Shared.Models.Timer;
using twitchDnd.Shared.Models.Voting;

namespace twitchDnd.Server.Services;

// ReSharper disable once UnusedType.Global
public class SessionService 
{
	private readonly IHubContext<ServerSignalRHub> _hub;
	private Timer _timer;

	public SessionService(IHubContext<ServerSignalRHub> hub)
	{
		_hub = hub;
		Session = new TimerSession();
	}

	public TimerSession Session { get; set; }
	private VotingResponsePayload CurrentVotingResponsePayload { get; set; } = VotingResponsePayload.DefaultPayload;
	private VotingResponsePayload NextVotingResponsePayload { get; set; } = VotingResponsePayload.DefaultPayload;

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
			_timer.Elapsed += TimerTick;
			_timer.Start();
			Session.Start();
			CurrentVotingResponsePayload = NextVotingResponsePayload;
			NextVotingResponsePayload = VotingResponsePayload.DefaultPayload;
			_hub.Clients?.All?.SendCoreAsync(HubMethods.TimerSession, new[] {Session});
			_hub.Clients?.All?.SendCoreAsync(HubMethods.VotingResponsePayload, new[] {CurrentVotingResponsePayload});
		}
	}
	 private void TimerTick(object sender, ElapsedEventArgs args)
	 {
		 var running = Session.AddSecond();
		 _hub.Clients?.All?.SendCoreAsync(HubMethods.TimerSession, new[] {Session});
		 if (!running)
		 {
			 StopTimer();
		 }
	 }

	public void StopTimer()
	{
		lock (this)
		{
			Session.Pause();
			_timer.Stop();
			CurrentVotingResponsePayload.CreateWinner();
			_hub.Clients?.All?.SendCoreAsync(HubMethods.TimerSession, new[] {Session});
			_hub.Clients?.All?.SendCoreAsync(HubMethods.VotingResponsePayload, new [] {CurrentVotingResponsePayload});
		}
	}
}