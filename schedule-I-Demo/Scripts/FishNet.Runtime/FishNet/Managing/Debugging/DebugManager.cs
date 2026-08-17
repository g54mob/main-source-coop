using UnityEngine;

namespace FishNet.Managing.Debugging
{
	[DisallowMultipleComponent]
	[AddComponentMenu("FishNet/Manager/DebugManager")]
	public class DebugManager : MonoBehaviour
	{
		public bool WriteSceneObjectDetails;

		public bool ObserverRpcLinks = true;

		public bool TargetRpcLinks = true;

		public bool ReplicateRpcLinks = true;

		public bool ReconcileRpcLinks = true;

		public bool ServerRpcLinks = true;
	}
}
