using System;
using Fusion;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	public interface IEnemyBehaviour : IEnemyTypeProvider
	{
		NetworkObject NetworkObject { get; }

		bool IsOccupySpawnPoint { get; }

		event Action<IEnemyBehaviour> OnDeath;

		void SetAreaPosition(Vector3 areaPosition);

		void BindSpawnPointOccupancy(IEnemySpawnPointOccupancy occupancy);

		void ReleaseSpawnPointOccupancy();
	}
}
