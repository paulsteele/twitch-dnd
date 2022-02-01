using System.Threading.Tasks;
using twitchDnd.Client.Services.Authentication;
using twitchDnd.Shared.Models;

namespace twitchDnd.Client.ViewModels.Account {
	public interface ILoginViewModel {
		public Task<LoginResult> Login(LoginModel loginModel);
	}

	public class LoginViewModel : ILoginViewModel {
		private readonly IAuthService _authService;
		public LoginViewModel(IAuthService authService) {
			_authService = authService;
		}

		public Task<LoginResult> Login(LoginModel loginModel) {
			return _authService.Login(loginModel);
		}
	}
}
