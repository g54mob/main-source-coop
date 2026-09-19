namespace Features.TutorialModule.Scripts.GuideModule
{
	public interface ITipPoolService
	{
		ITipEntity Pop(TipType tipType, bool animated = true);

		void Return(ITipEntity entity, bool animated = true);
	}
}
