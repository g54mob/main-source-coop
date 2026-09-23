using System;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerSpotState : NetworkBehaviour
	{
		private readonly NetworkVariable<bool> spotted = new NetworkVariable<bool>(value: false, NetworkVariableReadPermission.Owner);

		public bool IsSpotted => spotted.Value;

		public void SetSpotted(bool value)
		{
			if (base.IsServer && spotted.Value != value)
			{
				spotted.Value = value;
			}
		}

		public override void OnNetworkSpawn()
		{
			if (base.IsServer)
			{
				spotted.Value = false;
			}
		}

		protected override void __initializeVariables()
		{
			if (spotted == null)
			{
				throw new Exception("PlayerSpotState.spotted cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			spotted.Initialize(this);
			__nameNetworkVariable(spotted, "spotted");
			NetworkVariableFields.Add(spotted);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			base.__initializeRpcs();
		}

		protected internal override string __getTypeName()
		{
			return "PlayerSpotState";
		}
	}
}
