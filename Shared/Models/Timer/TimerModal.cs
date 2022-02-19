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

		public bool AddSecond()
		{
			ElapsedTime = ElapsedTime.Add(new TimeSpan(0, 0, 1));
			Running = !ElapsedTime.Equals(TotalTime);
			if (!Running)
			{
				ElapsedTime = TimeSpan.Zero;
			}
			return Running;
		}

		public void Start()
		{
			Running = true;
		}

		public void Pause()
		{
			Running = false;
		}

		public void Reset()
		{
			ElapsedTime = TimeSpan.Zero;
		}
	}
}