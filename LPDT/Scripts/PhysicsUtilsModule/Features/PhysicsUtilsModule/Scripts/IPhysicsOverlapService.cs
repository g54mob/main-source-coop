using UnityEngine;

namespace Features.PhysicsUtilsModule.Scripts
{
	public interface IPhysicsOverlapService
	{
		bool IsOverlapping(Collider collider, int layerMask, out Vector3 resolutionVector);

		void ResolveOverlap(Collider collider, int layerMask, int maxIterations = 5, float bias = 0.05f);

		void ResolveGroundOverlap(Collider collider, int groundLayerMask, float bias = 0.05f);
	}
}
