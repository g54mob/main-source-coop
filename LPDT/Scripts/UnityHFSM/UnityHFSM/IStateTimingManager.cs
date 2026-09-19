namespace UnityHFSM
{
	public interface IStateTimingManager
	{
		bool HasPendingTransition { get; }

		IStateTimingManager ParentFsm { get; }

		void StateCanExit();
	}
}
