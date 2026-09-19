namespace Features.TutorialModule.Scripts.GuideModule
{
	public interface IGuideLinePoolService
	{
		IGuideLineEntity Pop(GuideLineBuildType buildType, bool animated = true);

		void Return(IGuideLineEntity entity, bool animated = true);
	}
}
