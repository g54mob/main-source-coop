using System;
using Features.NetworkedModelRuntime;
using Features.SessionManagementModule.Models;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.SessionManagementModule.Networked
{
	[NetworkBehaviourWeaved(3)]
	public class SessionStateMachineNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Target", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SessionState _Target;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsTargetActive", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsTargetActive;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Epoch", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _Epoch;

		[Networked]
		[OnChangedRender("OnTargetChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe SessionState Target
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionStateMachineNetworkObject.Target. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(SessionState*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionStateMachineNetworkObject.Target. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(SessionState*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnIsTargetActiveChangedRender")]
		[NetworkedWeaved(1, 1)]
		public unsafe bool IsTargetActive
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionStateMachineNetworkObject.IsTargetActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionStateMachineNetworkObject.IsTargetActive. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		[Networked]
		[OnChangedRender("OnEpochChangedRender")]
		[NetworkedWeaved(2, 1)]
		public unsafe int Epoch
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionStateMachineNetworkObject.Epoch. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing SessionStateMachineNetworkObject.Epoch. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		public event Action<SessionState> OnNetworkedTargetChanged;

		public event Action<bool> OnNetworkedIsTargetActiveChanged;

		public event Action<int> OnNetworkedEpochChanged;

		public event Action OnAuthoritativeTick;

		public event Action OnDespawned;

		public event Action OnRender;

		[Inject]
		public void InjectDependencies(INetworkedModelInstanceProvider networkedModelInstanceProvider)
		{
			_networkedModelInstanceProvider = networkedModelInstanceProvider;
		}

		public override void Spawned()
		{
			_networkedModelInstanceProvider?.Register(this);
			this.OnNetworkedTargetChanged?.Invoke(Target);
			this.OnNetworkedIsTargetActiveChanged?.Invoke(IsTargetActive);
			this.OnNetworkedEpochChanged?.Invoke(Epoch);
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

		public override void Render()
		{
			this.OnRender?.Invoke();
		}

		public bool TryWriteTarget(SessionState target)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Target = target;
			return true;
		}

		public bool TryWriteIsTargetActive(bool isTargetActive)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			IsTargetActive = isTargetActive;
			return true;
		}

		public bool TryWriteEpoch(int epoch)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			Epoch = epoch;
			return true;
		}

		private void OnTargetChangedRender()
		{
			this.OnNetworkedTargetChanged?.Invoke(Target);
		}

		private void OnIsTargetActiveChangedRender()
		{
			this.OnNetworkedIsTargetActiveChanged?.Invoke(IsTargetActive);
		}

		private void OnEpochChangedRender()
		{
			this.OnNetworkedEpochChanged?.Invoke(Epoch);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			Target = _Target;
			IsTargetActive = _IsTargetActive;
			Epoch = _Epoch;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_Target = Target;
			_IsTargetActive = IsTargetActive;
			_Epoch = Epoch;
		}
	}
}
