using System;
using System.Collections.Generic;
using UnityEngine;

namespace Portningsbolaget.Platforms
{
	public class DebugPlatform : IPlatform
	{
		private class Progress
		{
			public int Current;

			public int Target;

			public bool IsUnlocked;
		}

		private Dictionary<Achievements, Progress> m_achievementProgress = new Dictionary<Achievements, Progress> { 
		{
			Achievements.ACH_WEEKLY_VIEWS_01,
			new Progress
			{
				Current = 0,
				Target = 1
			}
		} };

		public Action<string, string> OnJoinedSession { get; set; }

		public void InitializeAfterAssembliesLoaded()
		{
		}

		public void InitializeAfterSceneLoad()
		{
		}

		public void UnlockAchievement(Achievements achievement)
		{
			Debug.Log("[Platform] Unlocking Achievement: " + achievement);
		}

		public void ProgressAchievement(Achievements achievement)
		{
			if (!m_achievementProgress.TryGetValue(achievement, out var value))
			{
				Debug.LogError($"[Platform] Tried progressing a non progress achievement: {achievement}");
				return;
			}
			value.Current++;
			value.Current = Mathf.Clamp(value.Current, 0, value.Target);
			if (value.Current == value.Target)
			{
				Debug.Log($"[Platform] Unlocked Progress Achievement: {achievement}");
			}
			else
			{
				Debug.Log($"[Platform] Progressing Achievement: {achievement.ToString()} {value.Current}/{value.Target}");
			}
		}

		public void ClearAchievementsProgress()
		{
			Debug.Log("[Platform] Debug platform does not contain a definition for ClearAchievementsProgress");
		}

		public void OpenDialog(string title, DialogType dialogType, Action<string> onDone)
		{
			Debug.Log("[Platform] Opening Dialog: " + title);
		}

		public void Teardown()
		{
			Debug.Log("[Platform] Tearing down");
		}

		public void Update()
		{
		}
	}
}
