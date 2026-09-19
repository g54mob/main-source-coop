using Fusion.Photon.Realtime;
using Photon.Realtime;

namespace Fusion.Matchmaking
{
	public static class MatchmakingArgumentsExtensions
	{
		public static MatchmakingArguments SetupForFusion(this MatchmakingArguments arguments)
		{
			arguments.PluginName = "FusionPlugin21";
			arguments.MaxPlayers = NetworkProjectConfigAsset.Global.Config.Simulation.PlayerCount;
			ref AppSettings photonSettings = ref arguments.PhotonSettings;
			if (photonSettings == null)
			{
				photonSettings = PhotonAppSettings.Global.AppSettings;
			}
			arguments.NetworkClient = (arguments.NetworkClient ?? new RealtimeClient()).SetupForFusion(arguments.PhotonSettings as FusionAppSettings);
			return arguments;
		}

		public static RealtimeClient BuildRealtimeClient(FusionAppSettings config = null)
		{
			return new RealtimeClient().SetupForFusion(config ?? PhotonAppSettings.Global.AppSettings);
		}
	}
}
