using System;
using System.Collections.Generic;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModule.Scripts.Networked
{
	[NetworkBehaviourWeaved(106)]
	public class EnemySpawnStatesNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("SessionTimePassed", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _SessionTimePassed;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("States", 1, 105)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SerializableDictionary<EnemyType, EnemySpawnStateData> _States;

		[Networked]
		[OnChangedRender("OnSessionTimePassedChangedRender")]
		[NetworkedWeaved(0, 1)]
		public unsafe int SessionTimePassed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemySpawnStatesNetworkObject.SessionTimePassed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemySpawnStatesNetworkObject.SessionTimePassed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[Capacity(16)]
		[OnChangedRender("OnStatesChangedRender")]
		[NetworkedWeaved(1, 105)]
		[NetworkedWeavedDictionary(17, 1, 3, typeof(ElementReaderWriterUnmanaged<EnemyType, MetaConstant1>), typeof(ElementReaderWriterUnmanaged<EnemySpawnStateData, MetaConstant3>))]
		public unsafe NetworkDictionary<EnemyType, EnemySpawnStateData> States
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemySpawnStatesNetworkObject.States. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkDictionary<EnemyType, EnemySpawnStateData>(Ptr + 1, 17, ElementReaderWriterUnmanaged<EnemyType, MetaConstant1>.GetInstance(), ElementReaderWriterUnmanaged<EnemySpawnStateData, MetaConstant3>.GetInstance());
			}
		}

		public event Action<int> OnNetworkedSessionTimePassedChanged;

		public event Action<IReadOnlyDictionary<EnemyType, EnemySpawnStateData>> OnNetworkedStatesChanged;

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
			this.OnNetworkedSessionTimePassedChanged?.Invoke(SessionTimePassed);
			this.OnNetworkedStatesChanged?.Invoke(ReadStates());
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

		public bool TryWriteSessionTimePassed(int sessionTimePassed)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			SessionTimePassed = sessionTimePassed;
			return true;
		}

		public bool TryWriteStates(IReadOnlyDictionary<EnemyType, EnemySpawnStateData> states)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			List<EnemyType> list = null;
			foreach (KeyValuePair<EnemyType, EnemySpawnStateData> state in States)
			{
				if (!states.ContainsKey(state.Key))
				{
					(list ?? (list = new List<EnemyType>())).Add(state.Key);
				}
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					States.Remove(list[i]);
				}
			}
			foreach (KeyValuePair<EnemyType, EnemySpawnStateData> state2 in states)
			{
				States.Set(state2.Key, state2.Value);
			}
			return true;
		}

		private void OnSessionTimePassedChangedRender()
		{
			this.OnNetworkedSessionTimePassedChanged?.Invoke(SessionTimePassed);
		}

		private void OnStatesChangedRender()
		{
			this.OnNetworkedStatesChanged?.Invoke(ReadStates());
		}

		public Dictionary<EnemyType, EnemySpawnStateData> ReadStates()
		{
			Dictionary<EnemyType, EnemySpawnStateData> dictionary = new Dictionary<EnemyType, EnemySpawnStateData>(States.Count);
			foreach (KeyValuePair<EnemyType, EnemySpawnStateData> state in States)
			{
				dictionary[state.Key] = state.Value;
			}
			return dictionary;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			SessionTimePassed = _SessionTimePassed;
			NetworkBehaviourUtils.InitializeNetworkDictionary(States, _States, "States");
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SessionTimePassed = SessionTimePassed;
			NetworkBehaviourUtils.CopyFromNetworkDictionary(States, ref _States);
		}
	}
}
