using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using UnityEngine;

namespace FishNet.Component.Ownership
{
	public class PredictedOwner : NetworkBehaviour
	{
		[Tooltip("True if to enable this component.")]
		[SyncVar(SendRate = 0f)]
		[SerializeField]
		public bool _allowTakeOwnership = true;

		public SyncVar<bool> syncVar____allowTakeOwnership;

		private bool NetworkInitialize___EarlyFishNet_002EComponent_002EOwnership_002EPredictedOwnerFishNet_002ERuntime_002Edll_Excuted;

		private bool NetworkInitialize__LateFishNet_002EComponent_002EOwnership_002EPredictedOwnerFishNet_002ERuntime_002Edll_Excuted;

		public bool TakingOwnership { get; private set; }

		public NetworkConnection PreviousOwner { get; private set; } = NetworkManager.EmptyConnection;

		public bool SyncAccessor__allowTakeOwnership
		{
			get
			{
				return _allowTakeOwnership;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					_allowTakeOwnership = value;
				}
				if (Application.isPlaying)
				{
					syncVar____allowTakeOwnership.SetValue(value, value);
				}
			}
		}

		[Server]
		public void SetAllowTakeOwnership(bool value)
		{
			if (base.IsNetworked && !base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else
			{
				this.sync___set_value__allowTakeOwnership(value, true);
			}
		}

		public override void OnOwnershipClient(NetworkConnection prevOwner)
		{
			TakingOwnership = false;
			PreviousOwner = base.Owner;
		}

		[Client]
		public virtual void TakeOwnership()
		{
			if (base.IsNetworked && !base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else if (SyncAccessor__allowTakeOwnership && !base.IsOwner)
			{
				NetworkConnection connection = base.ClientManager.Connection;
				TakingOwnership = true;
				if (!base.IsServer)
				{
					base.NetworkObject.SetLocalOwnership(connection);
					ServerTakeOwnership();
				}
				else
				{
					OnTakeOwnership(connection);
				}
			}
		}

		[ServerRpc(RequireOwnership = false)]
		private void ServerTakeOwnership(NetworkConnection caller = null)
		{
			RpcWriter___Server_ServerTakeOwnership_328543758(caller);
		}

		[Server]
		protected virtual void OnTakeOwnership(NetworkConnection caller)
		{
			if (base.IsNetworked && !base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else if (caller.IsActive && SyncAccessor__allowTakeOwnership && !(caller == base.Owner))
			{
				GiveOwnership(caller);
			}
		}

		public virtual void NetworkInitialize___Early()
		{
			if (!NetworkInitialize___EarlyFishNet_002EComponent_002EOwnership_002EPredictedOwnerFishNet_002ERuntime_002Edll_Excuted)
			{
				NetworkInitialize___EarlyFishNet_002EComponent_002EOwnership_002EPredictedOwnerFishNet_002ERuntime_002Edll_Excuted = true;
				syncVar____allowTakeOwnership = new SyncVar<bool>(this, 0u, WritePermission.ServerOnly, ReadPermission.Observers, 0f, Channel.Reliable, _allowTakeOwnership);
				RegisterServerRpc(0u, RpcReader___Server_ServerTakeOwnership_328543758);
				RegisterSyncVarRead(ReadSyncVar___FishNet_002EComponent_002EOwnership_002EPredictedOwner);
			}
		}

		public virtual void NetworkInitialize__Late()
		{
			if (!NetworkInitialize__LateFishNet_002EComponent_002EOwnership_002EPredictedOwnerFishNet_002ERuntime_002Edll_Excuted)
			{
				NetworkInitialize__LateFishNet_002EComponent_002EOwnership_002EPredictedOwnerFishNet_002ERuntime_002Edll_Excuted = true;
				syncVar____allowTakeOwnership.SetRegistered();
			}
		}

		public override void NetworkInitializeIfDisabled()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}

		private void RpcWriter___Server_ServerTakeOwnership_328543758(NetworkConnection caller = null)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if ((object)networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if ((object)networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
			}
			else
			{
				Channel channel = Channel.Reliable;
				PooledWriter writer = WriterPool.GetWriter();
				SendServerRpc(0u, writer, channel, DataOrderType.Default);
				writer.Store();
			}
		}

		private void RpcLogic___ServerTakeOwnership_328543758(NetworkConnection caller = null)
		{
			OnTakeOwnership(caller);
		}

		private void RpcReader___Server_ServerTakeOwnership_328543758(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (base.IsServerInitialized)
			{
				RpcLogic___ServerTakeOwnership_328543758(conn);
			}
		}

		public virtual bool ReadSyncVar___FishNet_002EComponent_002EOwnership_002EPredictedOwner(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 0)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value__allowTakeOwnership(syncVar____allowTakeOwnership.GetValue(calledByUser: true), true);
					return true;
				}
				bool value = GeneratedReaders___Internal.InstancedExtension___ReadBoolean(PooledReader0);
				this.sync___set_value__allowTakeOwnership(value, Boolean2);
				return true;
			}
			return false;
		}

		public virtual void Awake()
		{
			NetworkInitialize___Early();
			NetworkInitialize__Late();
		}
	}
}
