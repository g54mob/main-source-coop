namespace Features.UINavigationModuleRealization.Scripts.BackButton
{
	public interface IBackButtonProcessor
	{
		BackButtonProcessorType Type { get; }

		bool CanHandleBack();

		void OnBack();
	}
}
