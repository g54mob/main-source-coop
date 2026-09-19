using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.RandomSoundPlayModule.Scripts
{
	[CreateAssetMenu(fileName = "RandomSoundPlayConfiguration_Default", menuName = "Configurations/RandomSoundPlayModule/RandomSoundPlayConfiguration")]
	public class RandomSoundPlayConfiguration : ScriptableObject
	{
		[SerializeField]
		[Min(0f)]
		[FormerlySerializedAs("<MaxKnockSoundsPerLevel>k__BackingField")]
		private int _defaultMaxSoundsPerLevel = 3;

		[SerializeField]
		private List<RandomSoundPlayLimit> _soundLimits = new List<RandomSoundPlayLimit>
		{
			new RandomSoundPlayLimit(RandomSoundType.DoorKnock, 3)
		};

		[field: SerializeField]
		[field: Min(0f)]
		public float EnemyProximityDistance { get; private set; } = 10f;

		[field: SerializeField]
		[field: Min(0.05f)]
		public float EnemyProximityCheckInterval { get; private set; } = 0.25f;

		public int GetMaxSoundsPerLevel(RandomSoundType soundType)
		{
			foreach (RandomSoundPlayLimit soundLimit in _soundLimits)
			{
				if (soundLimit.SoundType == soundType)
				{
					return soundLimit.MaxSoundsPerLevel;
				}
			}
			return _defaultMaxSoundsPerLevel;
		}
	}
}
