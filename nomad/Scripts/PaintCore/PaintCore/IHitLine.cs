using UnityEngine;

namespace PaintCore
{
	public interface IHitLine : IHit
	{
		void HandleHitLine(bool preview, int priority, float pressure, int seed, Vector3 position, Vector3 endPosition, Quaternion rotation, bool clip);
	}
}
