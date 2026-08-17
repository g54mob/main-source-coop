using UnityEngine;

namespace PaintCore
{
	public interface IHitCoord : IHit
	{
		void HandleHitCoord(bool preview, int priority, float pressure, int seed, CwHit hit, Quaternion rotation);
	}
}
