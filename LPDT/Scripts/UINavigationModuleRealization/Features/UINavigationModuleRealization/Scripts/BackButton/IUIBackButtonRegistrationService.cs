namespace Features.UINavigationModuleRealization.Scripts.BackButton
{
	public interface IUIBackButtonRegistrationService
	{
		void Register(IBackButtonProcessor processor);

		void Unregister(IBackButtonProcessor processor);
	}
}
