using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Shared
{
	public class ParentWithPosition
	{
		public Transform Parent;

		public Vector3 Position;

		public Quaternion Rotation;

		public ParentWithPosition(Transform parent, Vector3 position, Quaternion rotation)
		{
			Parent = parent;
			Position = position;
			Rotation = rotation;
		}
	}
}
