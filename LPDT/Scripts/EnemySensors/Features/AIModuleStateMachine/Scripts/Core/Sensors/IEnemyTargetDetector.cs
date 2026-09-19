using System;
using System.Collections.Generic;
using Fusion;

namespace Features.AIModuleStateMachine.Scripts.Core.Sensors
{
	public interface IEnemyTargetDetector
	{
		List<PlayerRef> DetectTargets { get; }

		event Action<PlayerRef> OnTargetDetectedInRange;

		event Action OnTargetDetecting;

		float GetDistanceToPlayer(PlayerRef player);

		bool IsPlayerDetected(PlayerRef player, out DetectionType detectionType);

		void EnableDetecting();

		void DisableDetecting();
	}
}
