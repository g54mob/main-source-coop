namespace RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core
{
	public interface IPresenterFactory
	{
		PresenterBehaviour GetPresenter(ViewBehaviour view);
	}
}
