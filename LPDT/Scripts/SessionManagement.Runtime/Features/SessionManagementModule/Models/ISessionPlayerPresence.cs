using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.SessionManagementModule.Models
{
	public interface ISessionPlayerPresence
	{
		bool IsAttached { get; }

		int PersistedLifeState { get; set; }

		SessionReconnectState ReconnectState { get; }

		UniTask PinAsync();

		UniTask AttachAsync();

		UniTask ReleaseAllAsync();

		UniTask WaitForReplicatedStateAsync();

		void SaveReconnectState(int levelId, Vector3 position, float yaw, bool isCrouching, float health);

		void SaveShopVote(int voteEpoch, bool hasVoted);

		bool HasShopVoteForEpoch(int currentEpoch);
	}
}
