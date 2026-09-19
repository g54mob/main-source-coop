using System;
using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public interface IGuideLineBuildService
	{
		GuideLineHandle StartTrackingPath(Transform startTransform, Transform endTransform, GuideLineBuildType buildType, GuideLineUpdateFrequencyType updateFrequencyType, Func<bool> killCondition = null);

		void StopTrackingPath(GuideLineHandle handle);
	}
}
