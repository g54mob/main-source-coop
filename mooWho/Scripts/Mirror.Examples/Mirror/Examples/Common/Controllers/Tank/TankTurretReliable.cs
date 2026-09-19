using UnityEngine;

namespace Mirror.Examples.Common.Controllers.Tank
{
	[AddComponentMenu("Network/Tank Turret (Reliable)")]
	[RequireComponent(typeof(TankControllerReliable))]
	[RequireComponent(typeof(NetworkTransformReliable))]
	public class TankTurretReliable : TankTurretBase
	{
		[Header("Network Transforms")]
		public NetworkTransformReliable turretNetworkTransform;

		public NetworkTransformReliable barrelNetworkTransform;

		protected override void Reset()
		{
			base.Reset();
			NetworkTransformReliable[] components = GetComponents<NetworkTransformReliable>();
			if (components.Length < 2)
			{
				turretNetworkTransform = base.gameObject.AddComponent<NetworkTransformReliable>();
				turretNetworkTransform.transform.SetSiblingIndex(components[0].transform.GetSiblingIndex() + 1);
				components = GetComponents<NetworkTransformReliable>();
			}
			else
			{
				turretNetworkTransform = components[1];
			}
			turretNetworkTransform.syncDirection = SyncDirection.ClientToServer;
			turretNetworkTransform.syncPosition = false;
			if (turret != null)
			{
				turretNetworkTransform.target = turret;
			}
			if (components.Length < 3)
			{
				barrelNetworkTransform = base.gameObject.AddComponent<NetworkTransformReliable>();
				barrelNetworkTransform.transform.SetSiblingIndex(components[1].transform.GetSiblingIndex() + 1);
				components = GetComponents<NetworkTransformReliable>();
			}
			else
			{
				barrelNetworkTransform = components[2];
			}
			barrelNetworkTransform.syncDirection = SyncDirection.ClientToServer;
			barrelNetworkTransform.syncPosition = false;
			if (barrel != null)
			{
				barrelNetworkTransform.target = barrel;
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
