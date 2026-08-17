using System;
using System.Collections;
using EvilAnalytics.SDK.Core;
using EvilAnalytics.Shared.BugReports;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BugReportExample : MonoBehaviour
{
	private bool _isReady;

	private string _statusText = "Waiting for SDK...";

	private string _bugMessage = "Something went wrong when I tried to open the inventory.";

	private IEnumerator Start()
	{
		while (EvilAnalyticsManager.Instance == null || !EvilAnalyticsManager.Instance.IsReady)
		{
			yield return null;
		}
		_isReady = true;
		_statusText = "Ready! Try submitting a bug report.";
		Debug.Log("[BugReportExample] Ready!");
	}

	public async void SubmitSimpleBugReport()
	{
		if (!_isReady)
		{
			return;
		}
		_statusText = "Sending bug report...";
		try
		{
			string sceneName = SceneManager.GetActiveScene().name;
			BugReportResponse bugReportResponse = await Analytics.SubmitBugReportAsync(_bugMessage, null, sceneName, Application.version);
			if (bugReportResponse != null && bugReportResponse.Success)
			{
				_statusText = "Bug report sent! ID: " + bugReportResponse.BugReportId.ToString().Substring(0, 8) + "...";
				Debug.Log($"[BugReport] Submitted: {bugReportResponse.BugReportId}");
			}
			else
			{
				_statusText = "Bug report failed to send.";
			}
		}
		catch (Exception ex)
		{
			_statusText = "Error: " + ex.Message;
			Debug.LogError("[BugReport] Failed: " + ex.Message);
		}
	}

	public void SubmitBugReportWithScreenshot()
	{
		if (!_isReady)
		{
			return;
		}
		_statusText = "Capturing screenshot and sending...";
		EvilAnalyticsManager.Instance.SubmitBugReport(_bugMessage, includeScreenshot: true, delegate(bool success, Guid? bugReportId)
		{
			if (success)
			{
				_statusText = "Bug report with screenshot sent! ID: " + bugReportId.ToString().Substring(0, 8) + "...";
				Debug.Log($"[BugReport] Submitted with screenshot: {bugReportId}");
			}
			else
			{
				_statusText = "Bug report failed to send.";
			}
		});
	}

	public void ShowBuiltInDialog()
	{
		_statusText = "Press F8 to open the built-in bug report dialog!";
		Debug.Log("[BugReport] Tip: Press F8 to open the built-in bug report dialog");
	}

	public void OnReportBugButtonClicked(string playerMessage)
	{
		if (!_isReady)
		{
			Debug.LogWarning("Analytics not ready yet");
			return;
		}
		EvilAnalyticsManager.Instance.SubmitBugReport(playerMessage, includeScreenshot: true, delegate(bool success, Guid? id)
		{
			if (success)
			{
				Debug.Log("Thank you for your bug report!");
			}
			else
			{
				Debug.Log("Failed to send bug report. Please try again.");
			}
		});
	}

	private void OnGUI()
	{
		if (_isReady)
		{
			GUILayout.BeginArea(new Rect(Screen.width - 310, 10f, 300f, 350f));
			GUILayout.BeginVertical("box");
			GUILayout.Label("Bug Report Example");
			GUILayout.Label(_statusText);
			GUILayout.Space(10f);
			GUILayout.Label("Bug description:");
			_bugMessage = GUILayout.TextArea(_bugMessage, GUILayout.Height(60f));
			GUILayout.Space(10f);
			if (GUILayout.Button("Submit (Text Only)"))
			{
				SubmitSimpleBugReport();
			}
			if (GUILayout.Button("Submit (With Screenshot)"))
			{
				SubmitBugReportWithScreenshot();
			}
			GUILayout.Space(10f);
			GUILayout.Label("Tip: Press F8 for built-in dialog");
			if (GUILayout.Button("Show F8 Hint"))
			{
				ShowBuiltInDialog();
			}
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}
