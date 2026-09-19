using System;
using UnityEngine;

namespace Features.RandomSoundPlayModule.Scripts
{
	[Serializable]
	public struct RandomSoundPlayLimit
	{
		[field: SerializeField]
		public RandomSoundType SoundType { get; private set; }

		[field: SerializeField]
		[field: Min(0f)]
		public int MaxSoundsPerLevel { get; private set; }

		public RandomSoundPlayLimit(RandomSoundType soundType, int maxSoundsPerLevel)
		{
			SoundType = soundType;
			MaxSoundsPerLevel = maxSoundsPerLevel;
		}
	}
}
