namespace Features.SessionManagementModule.Models
{
	public interface ISessionPlayerControl
	{
		void LockControlls();

		void UnlockControlls();

		void ApplyPersistedLifeState();

		void MirrorLifeStateToPersistence();
	}
}
