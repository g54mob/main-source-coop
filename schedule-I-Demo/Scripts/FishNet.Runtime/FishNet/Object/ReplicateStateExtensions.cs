namespace FishNet.Object
{
	public static class ReplicateStateExtensions
	{
		public static bool IsValid(this ReplicateState value)
		{
			return value != ReplicateState.Invalid;
		}

		public static bool IsReplayed(this ReplicateState value)
		{
			if (value != ReplicateState.ReplayedPredicted)
			{
				return value == ReplicateState.ReplayedUserCreated;
			}
			return true;
		}

		public static bool IsUserCreated(this ReplicateState value)
		{
			if (value != ReplicateState.UserCreated)
			{
				return value == ReplicateState.ReplayedUserCreated;
			}
			return true;
		}

		public static bool IsPredicted(this ReplicateState value)
		{
			return !value.IsUserCreated();
		}
	}
}
