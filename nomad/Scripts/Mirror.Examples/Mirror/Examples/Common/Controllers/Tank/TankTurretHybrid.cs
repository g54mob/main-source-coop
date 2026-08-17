using UnityEngine;

namespace Mirror.Examples.Common.Controllers.Tank
{
	[AddComponentMenu("Network/Tank Turret (Hybrid)")]
	[RequireComponent(typeof(TankControllerHybrid))]
	[RequireComponent(typeof(NetworkTransformHybrid))]
	public class TankTurretHybrid : TankTurretBase
	{
		[Header("Network Transforms")]
		public NetworkTransformHybrid turretNetworkTransform;

		public NetworkTransformHybrid barrelNetworkTransform;

		protected override void Reset()
		{
			base.Reset();
			NetworkTransformHybrid[] components = GetComponents<NetworkTransformHybrid>();
			if (components.Length < 2)
			{
				turretNetworkTransform = base.gameObject.AddComponent<NetworkTransformHybrid>();
				turretNetworkTransform.transform.SetSiblingIndex(components[0].transform.GetSiblingIndex() + 1);
				components = GetComponents<NetworkTransformHybrid>();
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
				barrelNetworkTransform = base.gameObject.AddComponent<NetworkTransformHybrid>();
				barrelNetworkTransform.transform.SetSiblingIndex(components[1].transform.GetSiblingIndex() + 1);
				components = GetComponents<NetworkTransformHybrid>();
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
