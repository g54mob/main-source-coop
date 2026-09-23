using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mimicraft.Gameplay
{
	public class ProbeVolumeRegionSwitcher : MonoBehaviour
	{
		private const float CheckInterval = 0.25f;

		private static ProbeVolumeRegionSwitcher instance;

		private float nextCheckTime;

		private ProbeVolumeRegion applied;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			instance = null;
		}

		public static void Ensure()
		{
			if (!(instance != null))
			{
				GameObject obj = new GameObject("ProbeVolumeRegionSwitcher")
				{
					hideFlags = HideFlags.HideAndDontSave
				};
				instance = obj.AddComponent<ProbeVolumeRegionSwitcher>();
				Object.DontDestroyOnLoad(obj);
			}
		}

		private void Update()
		{
			if (Time.unscaledTime < nextCheckTime)
			{
				return;
			}
			nextCheckTime = Time.unscaledTime + 0.25f;
			if (TryGetViewpoint(out var viewpoint))
			{
				ProbeVolumeRegion probeVolumeRegion = ProbeVolumeRegion.For(viewpoint);
				if (!(probeVolumeRegion == null) && !(probeVolumeRegion == applied) && !(probeVolumeRegion.BakingSet == null))
				{
					applied = probeVolumeRegion;
					ProbeReferenceVolume.instance?.SetActiveBakingSet(probeVolumeRegion.BakingSet);
				}
			}
		}

		private static bool TryGetViewpoint(out Vector3 viewpoint)
		{
			Camera main = Camera.main;
			if (main != null && main.isActiveAndEnabled)
			{
				viewpoint = main.transform.position;
				return true;
			}
			NetworkManager singleton = NetworkManager.Singleton;
			NetworkObject networkObject = ((!(singleton != null) || !singleton.IsListening) ? null : singleton.LocalClient?.PlayerObject);
			if (networkObject != null)
			{
				viewpoint = networkObject.transform.position;
				return true;
			}
			viewpoint = default(Vector3);
			return false;
		}
	}
}
