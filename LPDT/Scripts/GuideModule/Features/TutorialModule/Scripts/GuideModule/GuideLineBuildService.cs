using System;
using UnityEngine;

namespace Features.TutorialModule.Scripts.GuideModule
{
	public class GuideLineBuildService : IGuideLineBuildService
	{
		private readonly GuideLineBuilderDataHolder _guideLineBuilderDataHolder;

		public GuideLineBuildService(GuideLineBuilderDataHolder guideLineBuilderDataHolder)
		{
			_guideLineBuilderDataHolder = guideLineBuilderDataHolder;
		}

		public GuideLineHandle StartTrackingPath(Transform startTransform, Transform endTransform, GuideLineBuildType buildType, GuideLineUpdateFrequencyType updateFrequencyType, Func<bool> killCondition = null)
		{
			if (_guideLineBuilderDataHolder.GuideLineBuilder == null)
			{
				return GuideLineHandle.Invalid;
			}
			return _guideLineBuilderDataHolder.GuideLineBuilder.StartTrackingPath(startTransform, endTransform, buildType, updateFrequencyType, killCondition);
		}

		public void StopTrackingPath(GuideLineHandle handle)
		{
			if (!(_guideLineBuilderDataHolder.GuideLineBuilder == null) && handle.IsValid)
			{
				_guideLineBuilderDataHolder.GuideLineBuilder.StopTrackingPath(handle);
			}
		}
	}
}
