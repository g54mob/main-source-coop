using System;
using System.Collections;
using System.Collections.Generic;
using EvilAnalytics.SDK.Core;
using EvilAnalytics.Shared.Crashlytics;
using UnityEngine;

public class CrashlyticsExample : MonoBehaviour
{
	private bool _isReady;

	private string _statusText = "Waiting for SDK...";

	private string _logInfo = "";

	private IEnumerator Start()
	{
		while (EvilAnalyticsManager.Instance == null || !EvilAnalyticsManager.Instance.IsReady)
		{
			yield return null;
		}
		_isReady = true;
		_statusText = "Ready! Crashlytics captures errors automatically.";
		Debug.Log("[CrashlyticsExample] Ready! Errors will be automatically captured.");
	}

	public void TriggerTestException()
	{
		_statusText = "Exception triggered! Check dashboard for the error.";
		Debug.Log("[Crashlytics] Triggering a test exception...");
		try
		{
			throw new InvalidOperationException("Test exception from CrashlyticsExample - this is intentional!");
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	public void TriggerTestError()
	{
		_statusText = "Error logged! Will be sent to dashboard.";
		Debug.LogError("[TestError] Failed to load player profile - server returned 500");
	}

	public void TriggerTestWarning()
	{
		_statusText = "Warning logged! (captured if min severity is Warning)";
		Debug.LogWarning("[TestWarning] Low memory detected - consider freeing unused assets");
	}

	public void RecordManualLog()
	{
		if (_isReady)
		{
			Analytics.RecordLog("Custom error: Player inventory desync detected", "at GameManager.SyncInventory() in GameManager.cs:142\nat NetworkManager.OnDataReceived() in NetworkManager.cs:89", LogSeverity.Error);
			_statusText = "Manual log recorded!";
			Debug.Log("[Crashlytics] Manual log entry recorded");
		}
	}

	public void ViewRecentLogs()
	{
		if (!_isReady)
		{
			return;
		}
		List<LogEntry> recentCrashLogs = Analytics.GetRecentCrashLogs();
		if (recentCrashLogs == null || recentCrashLogs.Count == 0)
		{
			_logInfo = "No pending crash logs.";
			_statusText = "No pending logs";
			return;
		}
		_logInfo = $"Pending logs ({recentCrashLogs.Count}):\n";
		int num = 0;
		foreach (LogEntry item in recentCrashLogs)
		{
			if (num >= 5)
			{
				_logInfo += $"  ... and {recentCrashLogs.Count - 5} more\n";
				break;
			}
			_logInfo += $"  [{item.Severity}] {item.Message.Substring(0, Math.Min(50, item.Message.Length))}...\n";
			num++;
		}
		_statusText = $"{recentCrashLogs.Count} pending log(s)";
		Debug.Log($"[Crashlytics] {recentCrashLogs.Count} pending logs");
	}

	public async void FlushLogsNow()
	{
		if (!_isReady)
		{
			return;
		}
		_statusText = "Flushing logs...";
		try
		{
			bool flag = await Analytics.FlushCrashLogsAsync(Application.platform.ToString(), Application.version);
			_statusText = (flag ? "Logs sent to server!" : "No logs to send");
			Debug.Log("[Crashlytics] Flush result: " + (flag ? "sent" : "nothing to send"));
		}
		catch (Exception ex)
		{
			_statusText = "Flush failed: " + ex.Message;
			Debug.LogError("[Crashlytics] Flush failed: " + ex.Message);
		}
	}

	public void ConfigureExample()
	{
		if (_isReady)
		{
			Analytics.SetCrashlyticsMinSeverity(LogSeverity.Error);
			Analytics.SetCrashlyticsDeduplication(enabled: true);
			_statusText = "Config updated: Error+ only, dedup ON (5s window)";
			Debug.Log("[Crashlytics] Configuration updated");
		}
	}

	private void OnGUI()
	{
		if (_isReady)
		{
			GUILayout.BeginArea(new Rect(Screen.width - 310, 10f, 300f, 500f));
			GUILayout.BeginVertical("box");
			GUILayout.Label("Crashlytics Example");
			GUILayout.Label(_statusText);
			GUILayout.Space(10f);
			GUILayout.Label("Trigger Test Errors:");
			if (GUILayout.Button("Throw Exception"))
			{
				TriggerTestException();
			}
			if (GUILayout.Button("Log Error"))
			{
				TriggerTestError();
			}
			if (GUILayout.Button("Log Warning"))
			{
				TriggerTestWarning();
			}
			if (GUILayout.Button("Record Manual Log"))
			{
				RecordManualLog();
			}
			GUILayout.Space(10f);
			GUILayout.Label("View & Send:");
			if (GUILayout.Button("View Pending Logs"))
			{
				ViewRecentLogs();
			}
			if (GUILayout.Button("Flush Logs Now"))
			{
				FlushLogsNow();
			}
			GUILayout.Space(5f);
			if (!string.IsNullOrEmpty(_logInfo))
			{
				GUILayout.Label(_logInfo);
			}
			GUILayout.Space(10f);
			if (GUILayout.Button("Configure (Error+ only)"))
			{
				ConfigureExample();
			}
			GUILayout.Space(5f);
			GUILayout.Label("Tip: Errors are captured\nautomatically - no code needed!");
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}
