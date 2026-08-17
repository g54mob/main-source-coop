namespace NomadDrive.Features.ColliderLOD
{
	public interface IColliderLODManager
	{
		bool IsEnabled { get; }

		int ManagedCount { get; }

		void SetEnabled(bool enabled);
	}
}
