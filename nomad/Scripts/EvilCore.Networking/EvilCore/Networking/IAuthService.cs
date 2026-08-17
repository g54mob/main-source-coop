using System;

namespace EvilCore.Networking
{
	public interface IAuthService
	{
		bool IsLoggedIn { get; }

		bool SkipPlatformServices { get; }

		string DisplayName { get; }

		event Action OnLoginSuccess;

		event Action<string> OnLoginFailed;
	}
}
