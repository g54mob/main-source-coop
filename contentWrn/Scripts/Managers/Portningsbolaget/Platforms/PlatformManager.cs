using UnityEngine;

namespace Portningsbolaget.Platforms
{
	public static class PlatformManager
	{
		private static IPlatform PLATFORM;

		public static IPlatform Platform => PLATFORM;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void InitializePlatformSubsystemRegistration()
		{
			PlatformUtility.InitializePlatform();
			PLATFORM = new SteamRuntimeManager();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static void InitializePlatformAfterAssembliesLoaded()
		{
			PLATFORM.InitializeAfterAssembliesLoaded();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void InitializePlatformAfterSceneLoad()
		{
			PLATFORM.InitializeAfterSceneLoad();
		}

		public static void UnlockAchievement(Achievements achievement)
		{
			if (PLATFORM != null)
			{
				PLATFORM.UnlockAchievement(achievement);
			}
		}

		public static void ProgressAchievement(Achievements achievement)
		{
			Debug.Log($"Trying to progress achievement {achievement} - {PLATFORM}");
			if (PLATFORM != null)
			{
				PLATFORM.ProgressAchievement(achievement);
			}
		}

		public static void ProgressAchievement(Achievements achievement, int amount)
		{
			Debug.Log($"Trying to progress achievement {achievement} by {amount} - {PLATFORM}");
			if (PLATFORM != null)
			{
				PLATFORM.ProgressAchievement(achievement, amount);
			}
		}

		public static void SetAchievementProgress(Achievements achievement, int progress)
		{
			Debug.Log($"Trying to set achievement {achievement} to progress {progress} - {PLATFORM}");
			if (PLATFORM != null)
			{
				PLATFORM.SetAchievementProgress(achievement, progress);
			}
		}

		public static void ClearAchievementsProgress()
		{
			Debug.LogWarning("Clearing Achievements Progress");
			if (PLATFORM != null)
			{
				PLATFORM.ClearAchievementsProgress();
			}
		}

		public static void Update()
		{
			if (PLATFORM != null)
			{
				PLATFORM.Update();
			}
		}

		public static void TearDown()
		{
			if (PLATFORM != null)
			{
				PLATFORM.Teardown();
			}
		}
	}
}
