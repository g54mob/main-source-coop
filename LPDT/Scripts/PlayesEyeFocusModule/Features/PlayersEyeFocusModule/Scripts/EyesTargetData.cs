using UnityEngine;

namespace Features.PlayersEyeFocusModule.Scripts
{
	public class EyesTargetData
	{
		public readonly int Priority;

		public readonly Transform Transform;

		public EyesTargetData(int priority, Transform transform)
		{
			Priority = priority;
			Transform = transform;
		}
	}
}
