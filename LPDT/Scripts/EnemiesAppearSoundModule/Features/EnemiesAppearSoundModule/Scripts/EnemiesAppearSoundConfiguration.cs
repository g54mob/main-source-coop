using System.Collections.Generic;
using FMODUnity;
using Features.AIModule.Scripts;
using UnityEngine;

namespace Features.EnemiesAppearSoundModule.Scripts
{
	[CreateAssetMenu(fileName = "EnemiesAppearSoundConfiguration_Default", menuName = "Configurations/EnemiesAppearSound/EnemiesAppearSoundConfiguration")]
	public class EnemiesAppearSoundConfiguration : ScriptableObject
	{
		[field: SerializeField]
		public EventReference DefaultAppearSound { get; private set; }

		[field: SerializeField]
		public float MaxDistanceToPlayAppearSound { get; private set; } = 30f;

		[field: SerializeField]
		public float DistanceToResetAppearSound { get; private set; } = 40f;

		[field: SerializeField]
		public List<EnemyAppearSoundOverride> EnemyOverrides { get; private set; } = new List<EnemyAppearSoundOverride>();

		public EventReference GetAppearSound(EnemyType enemyType)
		{
			foreach (EnemyAppearSoundOverride enemyOverride in EnemyOverrides)
			{
				if (enemyOverride.EnemyType == enemyType && !enemyOverride.Sound.IsNull)
				{
					return enemyOverride.Sound;
				}
			}
			return DefaultAppearSound;
		}
	}
}
