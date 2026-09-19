using Features.Movement.Scripts;
using Fusion;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Flee
{
	public readonly struct EnemyFearFleePlayerCache
	{
		public PlayerRef PlayerRef { get; }

		public NetworkObject NetworkObject { get; }

		public PlayerLookDetection LookDetection { get; }

		public EnemyFearFleePlayerCache(PlayerRef playerRef, NetworkObject networkObject, PlayerLookDetection lookDetection)
		{
			PlayerRef = playerRef;
			NetworkObject = networkObject;
			LookDetection = lookDetection;
		}
	}
}
