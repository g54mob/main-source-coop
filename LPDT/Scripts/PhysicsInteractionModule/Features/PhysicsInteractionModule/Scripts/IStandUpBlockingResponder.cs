using UnityEngine;

namespace Features.PhysicsInteractionModule.Scripts
{
	public interface IStandUpBlockingResponder
	{
		void OnLocalPlayerStandUpBlocked(int inputAuthorityPlayerId, Vector3 playerWorldPosition, Collider standUpBlockingCollider);

		void OnLocalPlayerStandUpNoLongerBlocked(int inputAuthorityPlayerId, Collider standUpBlockingCollider);
	}
}
