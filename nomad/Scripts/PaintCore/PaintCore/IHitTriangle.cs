using UnityEngine;

namespace PaintCore
{
	public interface IHitTriangle : IHit
	{
		void HandleHitTriangle(bool preview, int priority, float pressure, int seed, Vector3 positionA, Vector3 positionB, Vector3 positionC, Quaternion rotation);
	}
}
