using Fusion;

namespace Features.ObjectDespawnModule.Scripts
{
	public class DespawnObjectData
	{
		public readonly NetworkObject TargetObject;

		public readonly bool Reliable;

		public DespawnObjectData(NetworkObject targetObject, bool reliable = false)
		{
			TargetObject = targetObject;
			Reliable = reliable;
		}
	}
}
