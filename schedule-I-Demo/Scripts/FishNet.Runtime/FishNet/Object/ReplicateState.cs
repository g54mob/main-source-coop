namespace FishNet.Object
{
	public enum ReplicateState : byte
	{
		Invalid = 0,
		UserCreated = 1,
		Predicted = 2,
		ReplayedUserCreated = 3,
		ReplayedPredicted = 4
	}
}
