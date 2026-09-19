using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.CartUpgradesModule.Scripts.Networked
{
	[NetworkBehaviourWeaved(1)]
	public class CartUpgradesNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("UpgradeModules", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _UpgradeModules;

		[Networked]
		[OnChangedRender("OnUpgradeModulesChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe int UpgradeModules
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CartUpgradesNetworkObject.UpgradeModules. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CartUpgradesNetworkObject.UpgradeModules. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		public event Action<int> OnNetworkedUpgradeModulesChanged;

		public event Action OnAuthoritativeTick;

		public event Action OnDespawned;

		[Inject]
		public void InjectDependencies(INetworkedModelInstanceProvider networkedModelInstanceProvider)
		{
			_networkedModelInstanceProvider = networkedModelInstanceProvider;
		}

		public override void Spawned()
		{
			_networkedModelInstanceProvider?.Register(this);
			this.OnNetworkedUpgradeModulesChanged?.Invoke(UpgradeModules);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_networkedModelInstanceProvider?.Unregister(this);
			this.OnDespawned?.Invoke();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				this.OnAuthoritativeTick?.Invoke();
			}
		}

		public bool TryWriteUpgradeModules(int upgradeModules)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			UpgradeModules = upgradeModules;
			return true;
		}

		private void OnUpgradeModulesChangedRender()
		{
			this.OnNetworkedUpgradeModulesChanged?.Invoke(UpgradeModules);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			UpgradeModules = _UpgradeModules;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_UpgradeModules = UpgradeModules;
		}
	}
}
