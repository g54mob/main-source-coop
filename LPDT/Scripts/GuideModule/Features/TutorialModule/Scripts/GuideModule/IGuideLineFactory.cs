namespace Features.TutorialModule.Scripts.GuideModule
{
	public interface IGuideLineFactory
	{
		IGuideLineEntity Create(GuideLineBuildType buildType);
	}
}
