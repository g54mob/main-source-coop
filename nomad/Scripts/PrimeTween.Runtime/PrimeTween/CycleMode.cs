using UnityEngine;

namespace PrimeTween
{
	public enum CycleMode : byte
	{
		[Tooltip("Restarts the animation from the beginning.")]
		Restart = 0,
		[Tooltip("Animates forth and back, like a yoyo. Easing is the same on the backward cycle.")]
		Yoyo = 1,
		[Tooltip("At the end of a cycle increments the `endValue` by the difference between `startValue` and `endValue`.\n\nFor example, if a tween moves position.x from 0 to 1, then after the first cycle, the tween will move the position.x from 1 to 2, and so on.")]
		Incremental = 2,
		[Tooltip("Rewinds the animation as if time was reversed. Easing is reversed on the backward cycle.")]
		Rewind = 3
	}
}
