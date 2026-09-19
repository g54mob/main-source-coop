namespace Features.TipsModule.Scripts.Views
{
	public interface ITipViewHost
	{
		TipViewBase RentSimple();

		CombineTipViewBase RentCombine();

		CombineTipViewBase RentSplit();

		void ReleaseAll();
	}
}
