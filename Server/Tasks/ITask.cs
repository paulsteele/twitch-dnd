using System.Threading.Tasks;

namespace twitchDnd.Server.Tasks;

public interface ITask
{
	public Task Start();
	public bool Completed { get; }
}