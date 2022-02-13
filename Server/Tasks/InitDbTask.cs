using System.Threading.Tasks;
using twitchDnd.Server.Commands;
using twitchDnd.Server.Database;

namespace twitchDnd.Server.Tasks;

// ReSharper disable once UnusedType.Global
public class InitDbTask : ITask
{
	private readonly IDb _db;
	private readonly EnsureUserHelper _ensureUserHelper;
	public bool Completed { get; private set; }

	public InitDbTask(IDb db, EnsureUserHelper ensureUserHelper)
	{
		_db = db;
		_ensureUserHelper = ensureUserHelper;
	}

	public async Task Start()
	{
		_db.Init();
		await _ensureUserHelper.EnsureUser();
		Completed = true;
	}
}