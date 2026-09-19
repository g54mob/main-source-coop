using System;
using Zenject;

namespace Features.LevelModule.Scripts
{
	public class LevelCleanupSystem : IInitializable, IDisposable
	{
		private readonly LevelModel _levelModel;

		private readonly ILevelCleanup[] _levelCleanups;

		public LevelCleanupSystem(LevelModel levelModel, ILevelCleanup[] levelCleanups)
		{
			_levelModel = levelModel;
			_levelCleanups = levelCleanups;
		}

		public void Initialize()
		{
			_levelModel.OnBeforeLevelLoaded += CleanupLevel;
		}

		public void Dispose()
		{
			_levelModel.OnBeforeLevelLoaded -= CleanupLevel;
		}

		private void CleanupLevel(LevelType _)
		{
			for (int i = 0; i < _levelCleanups.Length; i++)
			{
				_levelCleanups[i].Cleanup();
			}
		}
	}
}
