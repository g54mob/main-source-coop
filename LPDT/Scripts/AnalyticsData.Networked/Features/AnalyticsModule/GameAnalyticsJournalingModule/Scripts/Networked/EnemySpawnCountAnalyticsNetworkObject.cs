using System;
using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Networked
{
	[NetworkBehaviourWeaved(430)]
	public class EnemySpawnCountAnalyticsNetworkObject : NetworkBehaviour
	{
		private INetworkedModelInstanceProvider _networkedModelInstanceProvider;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("Counts", 0, 215)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SerializableDictionary<EnemyType, int> _Counts;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("KillCounts", 215, 215)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private SerializableDictionary<EnemyType, int> _KillCounts;

		[Networked]
		[Capacity(32)]
		[OnChangedRender("OnCountsChangedRender")]
		[NetworkedWeaved(0, 215)]
		[NetworkedWeavedDictionary(53, 1, 1, typeof(ElementReaderWriterUnmanaged<EnemyType, MetaConstant1>), typeof(ElementReaderWriterUnmanaged<int, MetaConstant1>))]
		public unsafe NetworkDictionary<EnemyType, int> Counts
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemySpawnCountAnalyticsNetworkObject.Counts. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkDictionary<EnemyType, int>((int*)((byte*)Ptr + 0), 53, ElementReaderWriterUnmanaged<EnemyType, MetaConstant1>.GetInstance(), ElementReaderWriterUnmanaged<int, MetaConstant1>.GetInstance());
			}
		}

		[Networked]
		[Capacity(32)]
		[OnChangedRender("OnKillCountsChangedRender")]
		[NetworkedWeaved(215, 215)]
		[NetworkedWeavedDictionary(53, 1, 1, typeof(ElementReaderWriterUnmanaged<EnemyType, MetaConstant1>), typeof(ElementReaderWriterUnmanaged<int, MetaConstant1>))]
		public unsafe NetworkDictionary<EnemyType, int> KillCounts
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing EnemySpawnCountAnalyticsNetworkObject.KillCounts. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkDictionary<EnemyType, int>(Ptr + 215, 53, ElementReaderWriterUnmanaged<EnemyType, MetaConstant1>.GetInstance(), ElementReaderWriterUnmanaged<int, MetaConstant1>.GetInstance());
			}
		}

		public event Action<IReadOnlyDictionary<EnemyType, int>> OnNetworkedCountsChanged;

		public event Action<IReadOnlyDictionary<EnemyType, int>> OnNetworkedKillCountsChanged;

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
			this.OnNetworkedCountsChanged?.Invoke(ReadCounts());
			this.OnNetworkedKillCountsChanged?.Invoke(ReadKillCounts());
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

		public bool TryWriteCounts(IReadOnlyDictionary<EnemyType, int> counts)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			List<EnemyType> list = null;
			foreach (KeyValuePair<EnemyType, int> count in Counts)
			{
				if (!counts.ContainsKey(count.Key))
				{
					(list ?? (list = new List<EnemyType>())).Add(count.Key);
				}
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Counts.Remove(list[i]);
				}
			}
			foreach (KeyValuePair<EnemyType, int> count2 in counts)
			{
				Counts.Set(count2.Key, count2.Value);
			}
			return true;
		}

		public bool TryWriteKillCounts(IReadOnlyDictionary<EnemyType, int> killCounts)
		{
			if (!base.HasStateAuthority)
			{
				return false;
			}
			List<EnemyType> list = null;
			foreach (KeyValuePair<EnemyType, int> killCount in KillCounts)
			{
				if (!killCounts.ContainsKey(killCount.Key))
				{
					(list ?? (list = new List<EnemyType>())).Add(killCount.Key);
				}
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					KillCounts.Remove(list[i]);
				}
			}
			foreach (KeyValuePair<EnemyType, int> killCount2 in killCounts)
			{
				KillCounts.Set(killCount2.Key, killCount2.Value);
			}
			return true;
		}

		private void OnCountsChangedRender()
		{
			this.OnNetworkedCountsChanged?.Invoke(ReadCounts());
		}

		private void OnKillCountsChangedRender()
		{
			this.OnNetworkedKillCountsChanged?.Invoke(ReadKillCounts());
		}

		public Dictionary<EnemyType, int> ReadCounts()
		{
			Dictionary<EnemyType, int> dictionary = new Dictionary<EnemyType, int>(Counts.Count);
			foreach (KeyValuePair<EnemyType, int> count in Counts)
			{
				dictionary[count.Key] = count.Value;
			}
			return dictionary;
		}

		public Dictionary<EnemyType, int> ReadKillCounts()
		{
			Dictionary<EnemyType, int> dictionary = new Dictionary<EnemyType, int>(KillCounts.Count);
			foreach (KeyValuePair<EnemyType, int> killCount in KillCounts)
			{
				dictionary[killCount.Key] = killCount.Value;
			}
			return dictionary;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			NetworkBehaviourUtils.InitializeNetworkDictionary(Counts, _Counts, "Counts");
			NetworkBehaviourUtils.InitializeNetworkDictionary(KillCounts, _KillCounts, "KillCounts");
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			NetworkBehaviourUtils.CopyFromNetworkDictionary(Counts, ref _Counts);
			NetworkBehaviourUtils.CopyFromNetworkDictionary(KillCounts, ref _KillCounts);
		}
	}
}
