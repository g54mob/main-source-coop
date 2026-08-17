using System;
using System.Collections;
using EvilAnalytics.SDK.Core;
using EvilAnalytics.Shared.Gdpr;
using UnityEngine;

public class GdprConsentExample : MonoBehaviour
{
	private bool _isReady;

	private string _statusText = "Waiting for SDK...";

	private string _consentInfo = "";

	private IEnumerator Start()
	{
		while (EvilAnalyticsManager.Instance == null || !EvilAnalyticsManager.Instance.IsReady)
		{
			yield return null;
		}
		_isReady = true;
		_statusText = "Ready!";
		UpdateConsentDisplay();
		Debug.Log("[GdprExample] Ready!");
	}

	public void GrantAllConsent()
	{
		if (_isReady)
		{
			Analytics.GrantConsent();
			_statusText = "All consents granted!";
			UpdateConsentDisplay();
			Debug.Log("[GDPR] All consents granted");
		}
	}

	public async void GrantSelectiveConsent()
	{
		if (_isReady)
		{
			await Analytics.SetConsentAsync(ConsentType.Analytics, granted: true);
			await Analytics.SetConsentAsync(ConsentType.CrashReporting, granted: true);
			await Analytics.SetConsentAsync(ConsentType.Hardware, granted: false);
			await Analytics.SetConsentAsync(ConsentType.Performance, granted: false);
			_statusText = "Selective consent set (Analytics + Crashes only)";
			UpdateConsentDisplay();
			Debug.Log("[GDPR] Selective consent: Analytics + CrashReporting only");
		}
	}

	public void RevokeAllConsent()
	{
		if (_isReady)
		{
			Analytics.RevokeConsent();
			_statusText = "All consents revoked - no data will be collected";
			UpdateConsentDisplay();
			Debug.Log("[GDPR] All consents revoked");
		}
	}

	public void CheckConsentStatus()
	{
		if (_isReady)
		{
			bool flag = Analytics.HasConsent(ConsentType.Analytics);
			bool flag2 = Analytics.HasConsent(ConsentType.Hardware);
			bool flag3 = Analytics.HasConsent(ConsentType.Performance);
			bool flag4 = Analytics.HasConsent(ConsentType.CrashReporting);
			Debug.Log($"[GDPR] Consent status: Analytics={flag}, Hardware={flag2}, " + $"Performance={flag3}, CrashReporting={flag4}");
			if (flag3)
			{
				Analytics.RecordFps(60f);
			}
		}
	}

	public async void ExportPlayerData()
	{
		if (!_isReady)
		{
			return;
		}
		_statusText = "Exporting player data...";
		Debug.Log("[GDPR] Requesting data export...");
		try
		{
			if (await Analytics.ExportDataAsync() != null)
			{
				_statusText = "Data exported! Check console for details.";
				Debug.Log("[GDPR] Data export received (check server response for full data)");
			}
			else
			{
				_statusText = "No data found for this player";
			}
		}
		catch (Exception ex)
		{
			_statusText = "Export failed: " + ex.Message;
			Debug.LogError("[GDPR] Export failed: " + ex.Message);
		}
	}

	public async void RequestAnonymization()
	{
		if (!_isReady)
		{
			return;
		}
		_statusText = "Requesting anonymization...";
		Debug.Log("[GDPR] Requesting data anonymization...");
		try
		{
			if (await Analytics.RequestAnonymizationAsync("Player requested deletion from settings menu") != null)
			{
				_statusText = "Data anonymized! Player data has been erased.";
				Debug.Log("[GDPR] Anonymization successful");
			}
			else
			{
				_statusText = "Anonymization request failed";
			}
		}
		catch (Exception ex)
		{
			_statusText = "Anonymization failed: " + ex.Message;
			Debug.LogError("[GDPR] Anonymization failed: " + ex.Message);
		}
	}

	private void UpdateConsentDisplay()
	{
		_consentInfo = "Consent Status:\n";
		_consentInfo = _consentInfo + "  Analytics:      " + (Analytics.HasConsent(ConsentType.Analytics) ? "YES" : "NO") + "\n";
		_consentInfo = _consentInfo + "  Hardware:       " + (Analytics.HasConsent(ConsentType.Hardware) ? "YES" : "NO") + "\n";
		_consentInfo = _consentInfo + "  Performance:    " + (Analytics.HasConsent(ConsentType.Performance) ? "YES" : "NO") + "\n";
		_consentInfo = _consentInfo + "  CrashReporting: " + (Analytics.HasConsent(ConsentType.CrashReporting) ? "YES" : "NO");
	}

	private void OnGUI()
	{
		if (_isReady)
		{
			GUILayout.BeginArea(new Rect(Screen.width - 310, 10f, 300f, 500f));
			GUILayout.BeginVertical("box");
			GUILayout.Label("GDPR Consent Example");
			GUILayout.Label(_statusText);
			GUILayout.Space(5f);
			GUILayout.Label(_consentInfo);
			GUILayout.Space(10f);
			GUILayout.Label("Consent Control:");
			if (GUILayout.Button("Accept All"))
			{
				GrantAllConsent();
			}
			if (GUILayout.Button("Analytics + Crashes Only"))
			{
				GrantSelectiveConsent();
			}
			if (GUILayout.Button("Decline All"))
			{
				RevokeAllConsent();
			}
			GUILayout.Space(10f);
			GUILayout.Label("Player Data Rights:");
			if (GUILayout.Button("Export My Data"))
			{
				ExportPlayerData();
			}
			if (GUILayout.Button("Delete My Data (Anonymize)"))
			{
				RequestAnonymization();
			}
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}
