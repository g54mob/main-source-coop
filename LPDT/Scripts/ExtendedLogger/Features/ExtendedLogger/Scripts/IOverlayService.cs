namespace Features.ExtendedLogger.Scripts
{
	public interface IOverlayService
	{
		void SwitchOverlay(DebugFilterType debugFilterType, object message);

		void SetOverlayActive(bool isActive);
	}
}
