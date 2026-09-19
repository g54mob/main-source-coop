using Fusion;
using UnityEngine;
using Zenject;

namespace Features.LevelModule.Scripts.RoomVariations
{
	[NetworkBehaviourWeaved(0)]
	public class RoomSpawner : NetworkBehaviour
	{
		[SerializeField]
		private RoomType _roomType;

		[SerializeField]
		private LevelType _levelType;

		[SerializeField]
		private Transform _spawnPositionReference;

		private IRoomSpawnService _roomSpawnService;

		private bool _isRoomSpawned;

		[Inject]
		private void InjectDependencies(IRoomSpawnService roomSpawnService, SpawnedRoomsModel spawnedRoomsModel)
		{
			_roomSpawnService = roomSpawnService;
		}

		public override async void Spawned()
		{
			base.Spawned();
			if (base.Runner.IsSharedModeMasterClient && _roomSpawnService.TryGetRandomVariationIndex(_levelType, _roomType, out var variationIndex))
			{
				await _roomSpawnService.SpawnRoom(_levelType, _roomType, _spawnPositionReference, variationIndex);
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
