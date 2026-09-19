namespace Features.UINavigationModuleRealization.Scripts.BackButton
{
	public class UIBackButtonRegistrationService : IUIBackButtonRegistrationService
	{
		private readonly UIBackButtonModel _model;

		public UIBackButtonRegistrationService(UIBackButtonModel model)
		{
			_model = model;
		}

		public void Register(IBackButtonProcessor processor)
		{
			_model.Register(processor);
		}

		public void Unregister(IBackButtonProcessor processor)
		{
			_model.Unregister(processor);
		}
	}
}
