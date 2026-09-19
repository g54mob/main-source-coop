using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModuleStateMachine.Scripts.Core.Contexts
{
	public interface IMovementContext
	{
		NavMeshAgent NavMeshAgent { get; }

		float MoveSpeed { get; }

		float SmoothedVelocity { get; }

		float SmoothedVelocityLerpSpeed { get; }

		Vector3 TargetPosition { get; }

		bool TargetPositionCompleted { get; }

		bool NeedToFindTargetPosition { get; set; }

		float CompletePointMinDistance { get; set; }

		int CurrentAreaType { get; }

		bool IsCrouching { get; }

		void SetMoveSpeed(float value);

		void SetSmoothedVelocity(float value);

		void SetTargetPosition(Vector3 position);

		void SetTargetPositionCompleted(bool isCompleted);

		void SetCurrentAreaType(int areaType);

		void SetIsCrouching(bool value);
	}
}
