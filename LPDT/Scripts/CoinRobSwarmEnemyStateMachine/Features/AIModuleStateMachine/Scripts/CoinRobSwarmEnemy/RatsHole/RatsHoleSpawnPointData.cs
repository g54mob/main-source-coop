using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.RatsHole
{
	public class RatsHoleSpawnPointData
	{
		public Vector3 Position { get; }

		public Quaternion Rotation { get; }

		public RatsHoleSpawnPointData(Vector3 position, Quaternion rotation)
		{
			Position = position;
			Rotation = rotation;
		}
	}
}
