using UnityEngine;

namespace Mimicraft.Tutorial
{
	public static class TutorialState
	{
		private const string CompletedKey = "Mimicraft.TutorialCompleted";

		private const string OfferedKey = "Mimicraft.TutorialOffered";

		private const string GraduateTipKey = "Mimicraft.TutorialGraduateTip";

		public static bool Completed => PlayerPrefs.GetInt("Mimicraft.TutorialCompleted", 0) == 1;

		public static bool Offered => PlayerPrefs.GetInt("Mimicraft.TutorialOffered", 0) == 1;

		public static bool ShouldOpenOnFirstRun
		{
			get
			{
				if (!Offered)
				{
					return !Completed;
				}
				return false;
			}
		}

		public static bool GraduateTipShown => PlayerPrefs.GetInt("Mimicraft.TutorialGraduateTip", 0) == 1;

		public static void MarkCompleted()
		{
			PlayerPrefs.SetInt("Mimicraft.TutorialCompleted", 1);
			PlayerPrefs.Save();
		}

		public static void MarkOffered()
		{
			PlayerPrefs.SetInt("Mimicraft.TutorialOffered", 1);
			PlayerPrefs.Save();
		}

		public static void MarkGraduateTipShown()
		{
			PlayerPrefs.SetInt("Mimicraft.TutorialGraduateTip", 1);
			PlayerPrefs.Save();
		}

		public static void ForgetGraduateTip()
		{
			PlayerPrefs.DeleteKey("Mimicraft.TutorialGraduateTip");
			PlayerPrefs.Save();
		}

		public static void ForgetOffered()
		{
			PlayerPrefs.DeleteKey("Mimicraft.TutorialOffered");
			PlayerPrefs.Save();
		}

		public static void ResetProgress()
		{
			PlayerPrefs.DeleteKey("Mimicraft.TutorialCompleted");
			PlayerPrefs.DeleteKey("Mimicraft.TutorialOffered");
			PlayerPrefs.DeleteKey("Mimicraft.TutorialGraduateTip");
			PlayerPrefs.Save();
		}
	}
}
