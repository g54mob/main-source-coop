using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.SessionManagementModule.Networked
{
	[NetworkBehaviourWeaved(3)]
	public class LevelNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("LevelId", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _LevelId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("BellStrikeCount", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _BellStrikeCount;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("CountdownStartTick", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _CountdownStartTick;

		[Networked]
		[OnChangedRender("OnLevelIdChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe int LevelId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelNetworkObject.LevelId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelNetworkObject.LevelId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnBellStrikeCountChangedRender")]
		[NetworkedWeaved(1, 1)]
		public unsafe int BellStrikeCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelNetworkObject.BellStrikeCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelNetworkObject.BellStrikeCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[OnChangedRender("OnCountdownStartTickChangedRender")]
		[NetworkedWeaved(2, 1)]
		public unsafe int CountdownStartTick
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelNetworkObject.CountdownStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing LevelNetworkObject.CountdownStartTick. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		public event Action<int> OnNetworkedLevelIdChanged;

		public event Action<int> OnNetworkedBellStrikeCountChanged;

		public event Action<int> OnNetworkedCountdownStartTickChanged;

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
			this.OnNetworkedLevelIdChanged?.Invoke(LevelId);
			this.OnNetworkedBellStrikeCountChanged?.Invoke(BellStrikeCount);
			this.OnNetworkedCountdownStartTickChanged?.Invoke(CountdownStartTick);
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

		public bool TryWriteLevelId(int levelId)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			LevelId = levelId;
			return true;
		}

		public bool TryWriteBellStrikeCount(int bellStrikeCount)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			BellStrikeCount = bellStrikeCount;
			return true;
		}

		public bool TryWriteCountdownStartTick(int countdownStartTick)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			CountdownStartTick = countdownStartTick;
			return true;
		}

		private void OnLevelIdChangedRender()
		{
			this.OnNetworkedLevelIdChanged?.Invoke(LevelId);
		}

		private void OnBellStrikeCountChangedRender()
		{
			this.OnNetworkedBellStrikeCountChanged?.Invoke(BellStrikeCount);
		}

		private void OnCountdownStartTickChangedRender()
		{
			this.OnNetworkedCountdownStartTickChanged?.Invoke(CountdownStartTick);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			LevelId = _LevelId;
			BellStrikeCount = _BellStrikeCount;
			CountdownStartTick = _CountdownStartTick;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_LevelId = LevelId;
			_BellStrikeCount = BellStrikeCount;
			_CountdownStartTick = CountdownStartTick;
		}
	}
}
