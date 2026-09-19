using System.Collections.Generic;
using Features.AIModule.Scripts;
using Features.NetworkedModelCodegen.Scripts;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data
{
	[NetworkedModel(ModelScope.Level, ModelOwnership.Shared)]
	public class EnemySpawnCountAnalyticsModel : NetworkedModelBase
	{
		[NetworkedCapacity(32)]
		public NetworkedDictionary<EnemyType, int> Counts { get; } = new NetworkedDictionary<EnemyType, int>();

		[NetworkedCapacity(32)]
		public NetworkedDictionary<EnemyType, int> KillCounts { get; } = new NetworkedDictionary<EnemyType, int>();

		public bool HasPendingSpawns => Counts.Count > 0;

		public bool HasPendingKills => KillCounts.Count > 0;

		public int PendingSpawnTypeCount => Counts.Count;

		public int PendingKillTypeCount => KillCounts.Count;

		public void RegisterSpawn(EnemyType enemyType)
		{
			if (base.IsAuthority && base.IsAttached)
			{
				int value2;
				int value = ((!Counts.TryGetValue(enemyType, out value2)) ? 1 : (value2 + 1));
				Counts.Set(enemyType, value);
			}
		}

		public void RegisterKill(EnemyType enemyType)
		{
			if (base.IsAuthority && base.IsAttached)
			{
				int value2;
				int value = ((!KillCounts.TryGetValue(enemyType, out value2)) ? 1 : (value2 + 1));
				KillCounts.Set(enemyType, value);
			}
		}

		public void CopyPendingSpawnsTo(List<KeyValuePair<EnemyType, int>> buffer)
		{
			buffer.Clear();
			foreach (KeyValuePair<EnemyType, int> item in Counts.Items)
			{
				buffer.Add(item);
			}
		}

		public void CopyPendingKillsTo(List<KeyValuePair<EnemyType, int>> buffer)
		{
			buffer.Clear();
			foreach (KeyValuePair<EnemyType, int> item in KillCounts.Items)
			{
				buffer.Add(item);
			}
		}

		public void ClearSpawns()
		{
			if (base.IsAuthority && base.IsAttached)
			{
				Counts.Clear();
			}
		}

		public void ClearKills()
		{
			if (base.IsAuthority && base.IsAttached)
			{
				KillCounts.Clear();
			}
		}
	}
}
