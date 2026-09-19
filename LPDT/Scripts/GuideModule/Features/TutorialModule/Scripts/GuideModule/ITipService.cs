using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public interface ITipService
	{
		TipHandle CreateTip(TipType tipType, Transform target, TipFollowFlags followFlags = TipFollowFlags.None, float upOffset = 0f, bool project = false);

		void KillTip(TipHandle handle);
	}
}
