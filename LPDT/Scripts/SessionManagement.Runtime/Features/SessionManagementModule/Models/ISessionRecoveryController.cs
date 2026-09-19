namespace Features.SessionManagementModule.Models
{
	public interface ISessionRecoveryController
	{
		void EvaluateAvatarPresence(bool isAvatarHeld);

		void RequestRecovery(RecoveryReason reason);
	}
}
