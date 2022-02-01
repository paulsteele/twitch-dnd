using System;
using System.Linq;
using System.Threading.Tasks;
using twitchDnd.Server.Database;
using Microsoft.AspNetCore.Identity;
using twitchDnd.Server.Configuration;

namespace twitchDnd.Server.Commands {
	public class EnsureUserHelper {
		private readonly IDb _db;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IEnvironmentVariableConfiguration _configuration;

		public EnsureUserHelper(IDb db, UserManager<IdentityUser> userManager, IEnvironmentVariableConfiguration configuration) {
			_db = db;
			_userManager = userManager;
			_configuration = configuration;
		}

		public Task EnsureUser()
		{
			return CreateUser(_configuration.DefaultUserName, _configuration.DefaultUserPass);
		}

		private async Task CreateUser(string username, string password) {
			var existingUser = _userManager.Users.FirstOrDefault(user => user.UserName == username);
			if (existingUser != null) {
				Console.WriteLine("User already exists");
				return;
			}

			var newUser = new IdentityUser { UserName = username };

			await _userManager.CreateAsync(newUser, password);
		}
	}
}
