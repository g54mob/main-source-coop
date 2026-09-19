using Cysharp.Threading.Tasks;
using Features.AIModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.TeleportModule.Scripts.TeleportCommon;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy
{
	public interface ICoinRobSwarmHost : IEnemyBehaviour, IEnemyTypeProvider, ITeleportable
	{
		bool IsInWanderingState { get; }

		bool HasActiveMembers { get; }

		bool CanReactToCoinFall { get; }

		bool IsNearChaseTarget(Vector3 worldPosition);

		void PrepareCoinChase(Vector3 target, IItem chaseItem);

		void CancelCoinChase();

		UniTask<bool> TryExtendSwarm();

		void ChaseTarget(Vector3 target, IItem chaseItem = null);
	}
}
