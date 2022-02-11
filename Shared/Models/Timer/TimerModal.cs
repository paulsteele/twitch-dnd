using System;

namespace twitchDnd.Shared.Models.Timer
{
	public class TimerModal
	{
		public TimeSpan TotalTime { get; set; }
		public TimeSpan ElapsedTime { get; set; }
		public TimeSpan RemainingTime => TotalTime - ElapsedTime;
		public bool Running { get; set; }

		public TimerModal(TimeSpan totalTime)
		{
			TotalTime = totalTime;
			ElapsedTime = TimeSpan.Zero;
			Running = false;
		}
	}
}