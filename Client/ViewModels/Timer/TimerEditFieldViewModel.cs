using System;
using twitchDnd.Shared.Models.Timer;

namespace twitchDnd.Client.ViewModels.Timer;

public class TimerEditFieldViewModel
{
	public TimerModal Modal { get; set; }

	public string TotalMinutes
	{
		get => Modal != null ? ((int) Math.Floor(Modal.TotalTime.TotalMinutes)).ToString() : String.Empty;
		set => SetTimerMinutes(Modal, value);
	}

	public string TotalSeconds
	{
		get => Modal != null ? Modal.TotalTime.Seconds.ToString() : String.Empty;
		set => SetTimerSeconds(Modal, value);
	}

	private void SetTimerMinutes(TimerModal modal, string value)
	{
		if (modal == null)
		{
			return;
		}
		if (int.TryParse(value, out var val) && val > 0)
		{
			modal.TotalTime = new TimeSpan(0, val, modal.TotalTime.Seconds);
		}
	}

	private void SetTimerSeconds(TimerModal modal, string value)
	{
		if (modal == null)
		{
			return;
		}

		if (int.TryParse(value, out var val) && val is > 0 and < 60)
		{
			modal.TotalTime = new TimeSpan(modal.TotalTime.Hours, modal.TotalTime.Minutes, val);
		}
	}
}