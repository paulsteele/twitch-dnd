using System;
using System.ComponentModel.DataAnnotations;

namespace twitchDnd.Shared.Models.Timer;


public enum TimerMode
{
	CollectingResponses,
	Voting,
	Stopped
}

public class TimerSession
{
	public TimerMode Mode { get; private set; } = TimerMode.CollectingResponses;
	public TimerModal ResponseCollectionTimer { get; set; } = new TimerModal(new TimeSpan(0, 0, 30));
	public TimerModal VotingTimer { get; set; } = new TimerModal(new TimeSpan(0, 0, 10));

	public void Start()
	{
		PerformAction(modal => modal.Start());
	}
	
	public void Pause()
	{
		PerformAction(modal => modal.Pause());
	}
	
	public bool Running => ResponseCollectionTimer.Running || VotingTimer.Running;

	public void Reset()
	{
		ResponseCollectionTimer.Reset();
		VotingTimer.Reset();
		Mode = TimerMode.CollectingResponses;
	}
	
	public TimeSpan RemainingTimeForCurrentMode => PerformAction(modal => modal.RemainingTime);

	public bool AddSecond()
	{
		return PerformAction(modal =>
			{
				var stillRunning = modal.AddSecond();
				if (stillRunning)
				{
					return true;
				}
				
				switch (Mode)
				{
					case TimerMode.CollectingResponses:
						Mode = TimerMode.Voting;
						VotingTimer.Start();
						return true;
					case TimerMode.Voting:
						Mode = TimerMode.Stopped;
						return false;
					default:
						return false;
				}
			}
		);
	}

	private void PerformAction(Action<TimerModal> action)
	{
		PerformAction(modal =>
		{
			action(modal);
			return true;
		});
	}
	
	private T PerformAction<T>(Func<TimerModal, T> action)
	{
		switch (Mode)
		{
			case TimerMode.CollectingResponses:
				return action(ResponseCollectionTimer);
			case TimerMode.Voting:
				return action(VotingTimer);
			default:
				return default;
		}
	}
}