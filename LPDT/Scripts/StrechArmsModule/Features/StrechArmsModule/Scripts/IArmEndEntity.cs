using Features.Movement.Scripts;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	public interface IArmEndEntity
	{
		Transform Transform { get; }

		StaticColorChangerByPlayerRef ColorChanger { get; }
	}
}
