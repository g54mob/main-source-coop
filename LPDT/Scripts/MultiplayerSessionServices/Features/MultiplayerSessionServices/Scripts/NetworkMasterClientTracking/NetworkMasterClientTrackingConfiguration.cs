using Fusion;
using UnityEngine;

namespace Features.MultiplayerSessionServices.Scripts.NetworkMasterClientTracking
{
	[CreateAssetMenu(fileName = "NetworkMasterClientTrackingConfiguration_Default", menuName = "Configurations/NetworkServices/NetworkMasterClientTrackingConfiguration")]
	public class NetworkMasterClientTrackingConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public NetworkPrefabRef NetworkMasterClientTrackerPrefab { get; private set; }
	}
}
