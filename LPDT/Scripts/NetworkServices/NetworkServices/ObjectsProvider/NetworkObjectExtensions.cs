using Fusion;

namespace NetworkServices.ObjectsProvider
{
	public static class NetworkObjectExtensions
	{
		public static void DespawnHierarchy(this NetworkObject root)
		{
			if (root == null || !root.IsValid || !root.HasStateAuthority || root.Runner == null)
			{
				return;
			}
			NetworkObject[] componentsInChildren = root.GetComponentsInChildren<NetworkObject>(includeInactive: true);
			foreach (NetworkObject networkObject in componentsInChildren)
			{
				if (!(networkObject == root) && networkObject.IsValid && networkObject.HasStateAuthority)
				{
					root.Runner.Despawn(networkObject);
				}
			}
			root.Runner.Despawn(root);
		}

		public static bool HasParentNetworkObject(this NetworkObject networkObject)
		{
			if (networkObject == null || networkObject.transform.parent == null)
			{
				return false;
			}
			return networkObject.transform.parent.GetComponentInParent<NetworkObject>(includeInactive: true) != null;
		}
	}
}
