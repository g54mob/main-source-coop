using System;
using Mimicraft.Localization;
using Mimicraft.Settings;
using Mimicraft.UI;
using UnityEngine;

namespace Mimicraft.Analytics
{
	public static class AnalyticsConsentPrompt
	{
		private const string AskedKey = "Mimicraft.AnalyticsConsentAsked";

		public static bool Answered => PlayerPrefs.GetInt("Mimicraft.AnalyticsConsentAsked", 0) == 1;

		public static bool ShouldAsk => !Answered;

		public static void Ask(Action continueWith)
		{
			if (Answered)
			{
				continueWith?.Invoke();
				return;
			}
			bool continued = false;
			try
			{
				if (DialogView.Instance == null)
				{
					Continue();
					return;
				}
				DialogView.Show(new DialogRequest(Loc.Get("Analytics.Consent.Body"), Loc.Get("Analytics.Consent.Accept"), Loc.Get("Analytics.Consent.Decline")), delegate(DialogAnswer answer, string _)
				{
					try
					{
						Record(answer == DialogAnswer.Confirm);
					}
					catch (Exception ex2)
					{
						Debug.LogError("[AnalyticsConsent] Cevap kaydedilemedi: " + ex2.Message);
					}
					Continue();
				});
			}
			catch (Exception ex)
			{
				Debug.LogError("[AnalyticsConsent] Soru sorulamadi, bir sonraki acilisa kaldi: " + ex.Message);
				Continue();
			}
			void Continue()
			{
				if (continued)
				{
					return;
				}
				continued = true;
				try
				{
					continueWith?.Invoke();
				}
				catch (Exception arg)
				{
					Debug.LogError($"[AnalyticsConsent] Devam adimi patladi: {arg}");
				}
			}
		}

		public static void Record(bool allow)
		{
			PlayerPrefs.SetInt("Mimicraft.AnalyticsConsentAsked", 1);
			PlayerPrefs.Save();
			GameSettings.SetAnalyticsConsent(allow);
		}

		public static void Forget()
		{
			PlayerPrefs.DeleteKey("Mimicraft.AnalyticsConsentAsked");
			PlayerPrefs.Save();
		}
	}
}
