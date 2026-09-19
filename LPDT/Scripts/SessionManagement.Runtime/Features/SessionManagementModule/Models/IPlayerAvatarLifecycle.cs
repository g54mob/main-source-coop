using Cysharp.Threading.Tasks;

namespace Features.SessionManagementModule.Models
{
	public interface IPlayerAvatarLifecycle
	{
		bool IsAvatarHeld { get; }

		UniTask EnsureSpawnedAsync(float initialHealth = -1f);

		void DespawnIfHeld();
	}
}
