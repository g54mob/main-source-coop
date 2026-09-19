namespace Features.SynchronizationGateModule
{
	public static class AuthorityGateResolver
	{
		public static bool IsOpen(int openedOnEpoch, int epoch)
		{
			if (epoch > 0)
			{
				return openedOnEpoch >= epoch;
			}
			return false;
		}

		public static int NextOpened(int openedOnEpoch, int epoch)
		{
			if (epoch <= openedOnEpoch)
			{
				return openedOnEpoch;
			}
			return epoch;
		}

		public static AuthorityGateAction Decide(bool isOpen, bool isAuthority)
		{
			if (isOpen)
			{
				return AuthorityGateAction.Pass;
			}
			if (!isAuthority)
			{
				return AuthorityGateAction.Wait;
			}
			return AuthorityGateAction.Execute;
		}
	}
}
