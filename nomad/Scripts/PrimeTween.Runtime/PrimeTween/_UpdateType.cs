using UnityEngine;

namespace PrimeTween
{
	internal enum _UpdateType : byte
	{
		[Tooltip("Uses 'PrimeTweenConfig.defaultUpdateType' to control the default Unity's event function, which updates the animation.")]
		Default = 0,
		[Tooltip("Updates the animation in MonoBehaviour.Update().\n\nIf the animation has 'startValue' and doesn't have a start delay, the 'startValue' is applied in 'PrimeTweenManager.LateUpdate'. This ensures the animation is rendered at the 'startValue' in the same frame it's created.")]
		Update = 1,
		[Tooltip("Updates the animation in MonoBehaviour.LateUpdate().\n\nIf the animation has 'startValue' and doesn't have a start delay, the 'startValue' is applied in 'PrimeTweenManager.LateUpdate'. This ensures the animation is rendered at the 'startValue' in the same frame it's created.")]
		LateUpdate = 2,
		[Tooltip("Updates the animation in 'MonoBehaviour.FixedUpdate()'.\n\nUnlike Update and LateUpdate animations, FixedUpdate animations don't apply the 'startValue' before the first frame is rendered. They receive their first update in the first FixedUpdate() after creation.")]
		FixedUpdate = 3
	}
}
