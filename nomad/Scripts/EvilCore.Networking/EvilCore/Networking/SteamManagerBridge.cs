using EvilCore.Networking.Steamworks.Scripts;
using VContainer;

namespace EvilCore.Networking
{
	public class SteamManagerBridge : OnlineServiceBridge
	{
		[Inject]
		private IAuthService _authService;

		private void Start()
		{
			if (!_authService.SkipPlatformServices)
			{
				MonoSingleton<SteamManager>.Instance?.Initialize();
			}
		}
	}
}
