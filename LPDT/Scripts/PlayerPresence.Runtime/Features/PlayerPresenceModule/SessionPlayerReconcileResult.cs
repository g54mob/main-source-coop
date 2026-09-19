namespace Features.PlayerPresenceModule
{
	public readonly struct SessionPlayerReconcileResult
	{
		public readonly SessionPlayerReconcileAction Action;

		public readonly int TargetIndex;

		public static SessionPlayerReconcileResult None => new SessionPlayerReconcileResult(SessionPlayerReconcileAction.None, -1);

		public SessionPlayerReconcileResult(SessionPlayerReconcileAction action, int targetIndex)
		{
			Action = action;
			TargetIndex = targetIndex;
		}
	}
}
