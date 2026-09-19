using Features.NetworkedModelCodegen.Scripts;

namespace Features.AIModule.Scripts
{
	[NetworkedModel(ModelScope.Level, ModelOwnership.Shared)]
	public class EnemySpawnStatesModel : NetworkedModelBase
	{
		public Networked<int> SessionTimePassed { get; } = new Networked<int>();

		[NetworkedCapacity(16)]
		public NetworkedDictionary<EnemyType, EnemySpawnStateData> States { get; } = new NetworkedDictionary<EnemyType, EnemySpawnStateData>();
	}
}
