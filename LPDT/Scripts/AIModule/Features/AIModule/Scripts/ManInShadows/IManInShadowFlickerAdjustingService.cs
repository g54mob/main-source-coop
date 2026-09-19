using UnityEngine;

namespace Features.AIModule.Scripts.ManInShadows
{
	public interface IManInShadowFlickerAdjustingService
	{
		void RegisterManInShadow(GameObject manInShadow);

		void SetInstanceBlend(GameObject manInShadow, float blend);

		void UnregisterManInShadow(GameObject manInShadow);
	}
}
