using UnityEngine;

namespace Mirror.Examples.Common.Controllers.Tank
{
	[AddComponentMenu("Network/Tank Turret (Unreliable)")]
	[RequireComponent(typeof(TankControllerUnreliable))]
	[RequireComponent(typeof(NetworkTransformUnreliable))]
	public class TankTurretUnreliable : TankTurretBase
	{
		[Header("Network Transforms")]
		public NetworkTransformUnreliable turretNetworkTransform;

		public NetworkTransformUnreliable barrelNetworkTransform;

		protected override void Reset()
		{
			base.Reset();
			NetworkTransformUnreliable[] components = GetComponents<NetworkTransformUnreliable>();
			if (components.Length < 2)
			{
				turretNetworkTransform = base.gameObject.AddComponent<NetworkTransformUnreliable>();
				turretNetworkTransform.transform.SetSiblingIndex(components[0].transform.GetSiblingIndex() + 1);
				components = GetComponents<NetworkTransformUnreliable>();
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
				barrelNetworkTransform = base.gameObject.AddComponent<NetworkTransformUnreliable>();
				barrelNetworkTransform.transform.SetSiblingIndex(components[1].transform.GetSiblingIndex() + 1);
				components = GetComponents<NetworkTransformUnreliable>();
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
