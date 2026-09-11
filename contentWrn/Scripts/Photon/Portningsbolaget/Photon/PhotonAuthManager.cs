using UnityEngine;

namespace Portningsbolaget.Photon
{
	public static class PhotonAuthManager
	{
		private static IPhotonAuth PHOTON_AUTH;

		public static IPhotonAuth PhotonAuth => PHOTON_AUTH;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void InitializePlatformSubsystemRegistration()
		{
			PHOTON_AUTH = new SteamPhotonAuthManager();
		}

		public static void Update()
		{
			PHOTON_AUTH?.Update();
		}

		public static void TearDown()
		{
			PHOTON_AUTH?.TearDown();
		}
	}
}
