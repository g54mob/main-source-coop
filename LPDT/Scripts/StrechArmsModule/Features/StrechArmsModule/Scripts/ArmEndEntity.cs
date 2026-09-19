using Features.Movement.Scripts;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	public class ArmEndEntity : IArmEndEntity
	{
		public Transform Transform { get; }

		public StaticColorChangerByPlayerRef ColorChanger { get; }

		public ArmEndEntity(Transform transform, StaticColorChangerByPlayerRef colorChanger)
		{
			Transform = transform;
			ColorChanger = colorChanger;
		}
	}
}
