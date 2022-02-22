using System;
using twitchDnd.Shared.Models.Timer;

namespace twitchDnd.Client.ViewModels.Timer;

public class TimerEditFieldViewModel
{
	public TimerModal Modal { get; set; }

	public string TotalMinutes
	{
		get => Modal == null ? string.Empty : ((int) Math.Floor(Modal.RemainingTime.TotalMinutes)).ToString();
		set => SetTimerMinutes(Modal, value);
	}

	public string TotalSeconds
	{
		get => Modal == null ? string.Empty : Modal.RemainingTime.Seconds.ToString();
		set => SetTimerSeconds(Modal, value);
	}

	private void SetTimerMinutes(TimerModal modal, string value)
	{
		if (modal == null)
		{
			return;
		}
		if (int.TryParse(value, out var val) && val >= 0)
		{
			modal.TotalTime = new TimeSpan(0, val, modal.RemainingTime.Seconds);
			modal.ElapsedTime = TimeSpan.Zero;
		}
	}

	private void SetTimerSeconds(TimerModal modal, string value)
	{
		if (modal == null)
		{
			return;
		}

		if (int.TryParse(value, out var val) && val is >= 0 and < 60)
		{
			modal.TotalTime = new TimeSpan(modal.RemainingTime.Hours, modal.RemainingTime.Minutes, val);
			modal.ElapsedTime = TimeSpan.Zero;
		}
	}
}