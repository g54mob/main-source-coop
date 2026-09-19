using Fusion;
using UnityEngine;

namespace Features.AIModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public abstract class EnemyMovableBase : NetworkBehaviour
	{
		public abstract void MoveToPoint(Vector3 position);

		public abstract bool IsCanMoveToPoint(Vector3 position);

		public abstract void Warp(Vector3 position);

		public abstract Vector3 GetVelocity();

		public abstract bool HasReachedEnd();

		public abstract bool HavePath();

		public abstract float GetStopingDistance();

		public abstract void ResetPath();

		public abstract void EnableNavMeshAgent(bool enable);

		public abstract void SetMovementSpeed(float speed);

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
