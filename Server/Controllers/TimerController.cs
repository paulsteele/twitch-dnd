using Microsoft.AspNetCore.Mvc;
using twitchDnd.Server.Tasks;
using twitchDnd.Shared.Models.Timer;

namespace twitchDnd.Server.Controllers {

	[ApiController]
	[Route("timer")]
	public class TimerController : ControllerBase {
		private readonly TimerTask _timerTask;

		public TimerController(
			TimerTask timerTask
		)
		{
			_timerTask = timerTask;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_timerTask.Session);
		}

		[Route("start")]
		[HttpPost]
		public IActionResult Start([FromBody]TimerSession timerSessionModal)
		{
			_timerTask.Session = timerSessionModal;
			_timerTask.StartTimer();
			return Ok();
		}
		
		[Route("stop")]
		[HttpPost]
		public IActionResult Stop() {
			_timerTask.StopTimer();
			return Ok();
		}
	}
}
