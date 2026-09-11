using System.Collections;
using Portningsbolaget;
using UnityEngine;
using UnityEngine.Networking;

public class CheckVersionHandler : RetrievableSingleton<CheckVersionHandler>
{
	public void CheckVersion()
	{
		StartCoroutine(CheckVersionCoroutine());
	}

	private IEnumerator CheckVersionCoroutine()
	{
		string matchmakingVersion = PlatformsConfig.GetMatchmakingVersion();
		string uri = "https://contentwarningloginfunctions.azurewebsites.net/api/VersionCheck?version=" + matchmakingVersion;
		using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
		yield return webRequest.SendWebRequest();
		if (webRequest.result == UnityWebRequest.Result.Success)
		{
			CheckResult(webRequest.downloadHandler.text);
		}
	}

	private void CheckResult(string result)
	{
		if (result != "VersionOK")
		{
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.OldVersion);
			string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.PleaseUpdate);
			string localizedString3 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.CloseGame);
			Modal.Show(localizedString, localizedString2, new ModalOption[1]
			{
				new ModalOption(localizedString3)
			}, OnClosed);
		}
	}

	private void OnClosed()
	{
		Application.Quit();
	}
}
