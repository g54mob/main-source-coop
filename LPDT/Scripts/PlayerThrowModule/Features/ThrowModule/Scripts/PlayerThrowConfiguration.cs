using UnityEngine;

namespace Features.ThrowModule.Scripts
{
	[CreateAssetMenu(fileName = "PlayerThrowConfiguration_Default", menuName = "Configurations/PlayerThrowModule/PlayerThrowConfiguration")]
	public class PlayerThrowConfiguration : ScriptableObject
	{
		public PhysicsMaterial ThrowPhysicsMaterial;
	}
}
