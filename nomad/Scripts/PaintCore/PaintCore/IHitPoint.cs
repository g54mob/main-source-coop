using UnityEngine;

namespace PaintCore
{
	public interface IHitPoint : IHit
	{
		void HandleHitPoint(bool preview, int priority, float pressure, int seed, Vector3 position, Quaternion rotation);
	}
}
