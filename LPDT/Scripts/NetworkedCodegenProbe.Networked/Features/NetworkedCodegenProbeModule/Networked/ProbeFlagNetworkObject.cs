using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.NetworkedCodegenProbeModule.Networked
{
	[NetworkBehaviourWeaved(1)]
	public class ProbeFlagNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Level", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private float _Level;

		[Networked]
		[OnChangedRender("OnLevelChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe float Level
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ProbeFlagNetworkObject.Level. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(float*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing ProbeFlagNetworkObject.Level. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(float*)((byte*)Ptr + 0) = value;
			}
		}

		public event Action<float> OnNetworkedLevelChanged;

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
			this.OnNetworkedLevelChanged?.Invoke(Level);
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

		public bool TryWriteLevel(float level)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Level = level;
			return true;
		}

		private void OnLevelChangedRender()
		{
			this.OnNetworkedLevelChanged?.Invoke(Level);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Level = _Level;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Level = Level;
		}
	}
}
