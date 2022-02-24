using Microsoft.AspNetCore.Mvc;
using twitchDnd.Server.Services;
using twitchDnd.Server.Tasks;
using twitchDnd.Shared.Models.Timer;

namespace twitchDnd.Server.Controllers {

	[ApiController]
	[Route("timer")]
	public class TimerController : ControllerBase {
		private readonly SessionService _sessionService;

		public TimerController(
			SessionService sessionService
		)
		{
			_sessionService = sessionService;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_sessionService.Session);
		}

		[Route("start")]
		[HttpPost]
		public IActionResult Start([FromBody]TimerSession timerSessionModal)
		{
			_sessionService.Session = timerSessionModal;
			_sessionService.StartTimer();
			return Ok();
		}
		
		[Route("stop")]
		[HttpPost]
		public IActionResult Stop() {
			_sessionService.StopTimer();
			return Ok();
		}
	}
}
