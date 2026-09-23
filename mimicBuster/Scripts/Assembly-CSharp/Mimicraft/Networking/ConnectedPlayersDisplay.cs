using System;
using Mimicraft.UI;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Networking
{
	[RequireComponent(typeof(NetworkObject))]
	public class ConnectedPlayersDisplay : NetworkBehaviour
	{
		[SerializeField]
		private LobbyInfoView infoView;

		private readonly NetworkVariable<int> connectedCount = new NetworkVariable<int>(0);

		public void SetInfoView(LobbyInfoView infoView)
		{
			this.infoView = infoView;
		}

		public override void OnNetworkSpawn()
		{
			NetworkVariable<int> networkVariable = connectedCount;
			networkVariable.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Combine(networkVariable.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(OnCountChanged));
			OnCountChanged(0, connectedCount.Value);
			if (base.IsServer)
			{
				RefreshCount();
				base.NetworkManager.OnClientConnectedCallback += OnClientJoinedOrLeft;
				base.NetworkManager.OnClientDisconnectCallback += OnClientJoinedOrLeft;
			}
		}

		public override void OnNetworkDespawn()
		{
			NetworkVariable<int> networkVariable = connectedCount;
			networkVariable.OnValueChanged = (NetworkVariable<int>.OnValueChangedDelegate)Delegate.Remove(networkVariable.OnValueChanged, new NetworkVariable<int>.OnValueChangedDelegate(OnCountChanged));
			if (base.IsServer && base.NetworkManager != null)
			{
				base.NetworkManager.OnClientConnectedCallback -= OnClientJoinedOrLeft;
				base.NetworkManager.OnClientDisconnectCallback -= OnClientJoinedOrLeft;
			}
		}

		private void OnClientJoinedOrLeft(ulong clientId)
		{
			RefreshCount();
		}

		private void RefreshCount()
		{
			connectedCount.Value = base.NetworkManager.ConnectedClients.Count;
		}

		private void OnCountChanged(int previous, int current)
		{
			if (infoView != null)
			{
				infoView.SetConnectedCount(current);
			}
		}

		protected override void __initializeVariables()
		{
			if (connectedCount == null)
			{
				throw new Exception("ConnectedPlayersDisplay.connectedCount cannot be null. All NetworkVariableBase instances must be initialized.");
			}
			connectedCount.Initialize(this);
			__nameNetworkVariable(connectedCount, "connectedCount");
			NetworkVariableFields.Add(connectedCount);
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			base.__initializeRpcs();
		}

		protected internal override string __getTypeName()
		{
			return "ConnectedPlayersDisplay";
		}
	}
}
