using System;
using Mimicraft;
using Mimicraft.Analytics;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.Tutorial;
using Mimicraft.UI;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI versionText;

	private static string VersionLabel => Build.VersionLabel;

	private void Awake()
	{
		if ((bool)versionText)
		{
			versionText.text = Loc.Format("Common.Version", VersionLabel);
		}
	}

	private void Start()
	{
		if (SplashScreenView.IsShowing)
		{
			SplashScreenView.Dismissed += GreetOnFirstRun;
		}
		else
		{
			GreetOnFirstRun();
		}
	}

	private void GreetOnFirstRun()
	{
		if (AnalyticsConsentPrompt.ShouldAsk)
		{
			AnalyticsConsentPrompt.Ask(GreetAfterConsent);
		}
		else
		{
			GreetAfterConsent();
		}
	}

	private void GreetAfterConsent()
	{
		string text = DisconnectNotice.Take();
		if (text != null)
		{
			DialogView.Show(new DialogRequest(Loc.Get(text), Loc.Get("Common.Close")), null);
		}
		Telemetry.Send("menu_reached", ("session_seconds", Telemetry.SessionSeconds), ("after_disconnect", text != null));
		if (QuickPlay.ConsumeMenuRequest())
		{
			QuickPlay.Start(base.gameObject.scene);
		}
		else if (!AboutView.TryShowForNewBuild((text == null) ? new Action(ShowGraduateTip) : null) && text == null)
		{
			ShowGraduateTip();
		}
	}

	private static void ShowGraduateTip()
	{
		if (TutorialState.Completed && !TutorialState.GraduateTipShown)
		{
			TutorialState.MarkGraduateTipShown();
			DialogView.Show(new DialogRequest(Loc.Get("Tip.Customization"), Loc.Get("Common.Ok")), null);
		}
	}

	public void PracticeButton()
	{
		PracticeSession.Start(tutorial: false);
	}

	public void TutorialButton()
	{
		PracticeSession.Start(tutorial: true);
	}

	public void QuitButton()
	{
		Application.Quit();
	}
}
