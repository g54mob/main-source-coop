using UnityEngine;

namespace Features.LevelModule.Scripts.LevelTransition
{
	[CreateAssetMenu(fileName = "LevelTransitionConfiguration_Default", menuName = "Configurations/LevelTransitionModule/LevelTransitionConfiguration")]
	public class LevelTransitionConfiguration : ScriptableObject
	{
		public float MaxTimer = 10f;
	}
}
