using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Features.BootstrapModule.Scripts
{
	public interface IPlayerSpawnCoordinationService
	{
		UniTask WaitForSpawnClearanceAsync(PlayerRef localPlayer, Vector3 spawnPosition, bool hasReconnectSpawnPosition = false);
	}
}
