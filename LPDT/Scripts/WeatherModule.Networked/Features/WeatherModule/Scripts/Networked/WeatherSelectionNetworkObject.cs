using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.WeatherModule.Scripts.Networked
{
	[NetworkBehaviourWeaved(2)]
	public class WeatherSelectionNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Selection", 0, 2)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private long _Selection;

		[Networked]
		[OnChangedRender("OnSelectionChangedRender")]
		[NetworkedWeaved(0, 2)]
		public unsafe long Selection
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing WeatherSelectionNetworkObject.Selection. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(long*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing WeatherSelectionNetworkObject.Selection. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(long*)((byte*)Ptr + 0) = value;
			}
		}

		public event Action<long> OnNetworkedSelectionChanged;

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
			this.OnNetworkedSelectionChanged?.Invoke(Selection);
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

		public bool TryWriteSelection(long selection)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Selection = selection;
			return true;
		}

		private void OnSelectionChangedRender()
		{
			this.OnNetworkedSelectionChanged?.Invoke(Selection);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Selection = _Selection;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Selection = Selection;
		}
	}
}
