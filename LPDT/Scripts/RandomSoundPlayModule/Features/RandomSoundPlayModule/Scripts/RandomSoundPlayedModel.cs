using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.LevelModule.Scripts;

namespace Features.RandomSoundPlayModule.Scripts
{
	public class RandomSoundPlayedModel : ISessionCleanup, ILevelCleanup
	{
		private readonly Dictionary<RandomSoundType, int> _playedCounts = new Dictionary<RandomSoundType, int>();

		public IReadOnlyDictionary<RandomSoundType, int> PlayedCounts => _playedCounts;

		public int GetPlayedCount(RandomSoundType soundType)
		{
			if (!_playedCounts.TryGetValue(soundType, out var value))
			{
				return 0;
			}
			return value;
		}

		public bool IsLimitReached(RandomSoundType soundType, int maxSoundsPerLevel)
		{
			return GetPlayedCount(soundType) >= maxSoundsPerLevel;
		}

		public bool TryRegisterPlayed(RandomSoundType soundType, int maxSoundsPerLevel)
		{
			if (IsLimitReached(soundType, maxSoundsPerLevel))
			{
				return false;
			}
			_playedCounts[soundType] = GetPlayedCount(soundType) + 1;
			return true;
		}

		public void Cleanup()
		{
			_playedCounts.Clear();
		}
	}
}
