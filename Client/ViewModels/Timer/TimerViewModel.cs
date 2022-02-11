using System;
using System.Globalization;
using twitchDnd.Shared.Bases;
using twitchDnd.Shared.Models.Timer;

namespace twitchDnd.Client.ViewModels.Timer
{
	public class TimerViewModel : BaseNotifyStateChanged
	{
		private readonly TimerModal _modal = new(TimeSpan.Zero);
		public string TimerValue => _modal.RemainingTime.ToString(@"mm\:ss");

		public string TotalMinutes
		{
			get => ((int) Math.Floor(_modal.TotalTime.TotalMinutes)).ToString();
			set
			{
				if (int.TryParse(value, out var val) && val > 0)
				{
					_modal.TotalTime = new TimeSpan(0, val, _modal.TotalTime.Seconds);
				}
			}
		}

		public string TotalSeconds
		{
			get => _modal.TotalTime.Seconds.ToString();
			set
			{
				if (int.TryParse(value, out var val) && val is > 0 and < 60)
				{
					_modal.TotalTime = new TimeSpan(0, _modal.TotalTime.Minutes, val);
				}
			}
		}

		public bool Editing => !_modal.Running;
	}
}