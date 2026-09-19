using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.LevelModule.Scripts.RoomVariations
{
	public interface IRoomSpawnService
	{
		UniTask<bool> SpawnRoom(LevelType levelType, RoomType roomType, Transform positionReference, int variationIndex);

		bool TryGetRandomVariationIndex(LevelType levelType, RoomType roomType, out int variationIndex);
	}
}
