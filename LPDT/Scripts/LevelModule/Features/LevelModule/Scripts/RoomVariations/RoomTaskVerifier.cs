using Fusion;
using Zenject;

namespace Features.LevelModule.Scripts.RoomVariations
{
	[NetworkBehaviourWeaved(0)]
	public class RoomTaskVerifier : NetworkBehaviour
	{
		private SpawnedRoomsModel _spawnedRoomsModel;

		[Inject]
		public void InjectDependencies(SpawnedRoomsModel spawnedRoomsModel)
		{
			_spawnedRoomsModel = spawnedRoomsModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			SpawnedRoomsModel spawnedRoomsModel = _spawnedRoomsModel;
			int spawnedRoomsCount = spawnedRoomsModel.SpawnedRoomsCount + 1;
			spawnedRoomsModel.SpawnedRoomsCount = spawnedRoomsCount;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			if (!runner.IsShutdown)
			{
				SpawnedRoomsModel spawnedRoomsModel = _spawnedRoomsModel;
				int spawnedRoomsCount = spawnedRoomsModel.SpawnedRoomsCount - 1;
				spawnedRoomsModel.SpawnedRoomsCount = spawnedRoomsCount;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
