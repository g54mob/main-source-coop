using System.Collections;
using EvilAnalytics.SDK.Core;
using EvilAnalytics.Shared.Events;
using UnityEngine;
using UnityEngine.Profiling;

public class PerformanceTrackingExample : MonoBehaviour
{
	[Header("Display Settings")]
	[Tooltip("Show FPS overlay on screen")]
	[SerializeField]
	private bool showFpsOverlay = true;

	[Tooltip("Update interval for the overlay (seconds)")]
	[SerializeField]
	private float updateInterval = 0.5f;

	private bool _isReady;

	private float _timer;

	private float _currentFps;

	private float _currentMemoryMb;

	private string _summaryText = "No data yet";

	private IEnumerator Start()
	{
		while (EvilAnalyticsManager.Instance == null || !EvilAnalyticsManager.Instance.IsReady)
		{
			yield return null;
		}
		_isReady = true;
		Debug.Log("[PerformanceExample] Ready! FPS is being tracked automatically.");
	}

	private void Update()
	{
		if (_isReady)
		{
			_currentFps = 1f / Time.unscaledDeltaTime;
			_timer += Time.deltaTime;
			if (_timer >= updateInterval)
			{
				_timer = 0f;
				_currentMemoryMb = (float)Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
			}
		}
	}

	public void ShowPerformanceSummary()
	{
		if (!_isReady)
		{
			return;
		}
		SessionPerformanceSummary currentPerformanceSummary = Analytics.GetCurrentPerformanceSummary();
		if (currentPerformanceSummary == null || currentPerformanceSummary.SampleCount == 0)
		{
			_summaryText = "No performance data collected yet";
			return;
		}
		_summaryText = "Performance Summary:\n";
		_summaryText += $"  Average FPS: {currentPerformanceSummary.AvgFps:F1}\n";
		_summaryText += $"  Min FPS:     {currentPerformanceSummary.MinFps:F1}\n";
		_summaryText += $"  Max FPS:     {currentPerformanceSummary.MaxFps:F1}\n";
		_summaryText += $"  1% Low FPS:  {currentPerformanceSummary.P1Fps:F1}\n";
		_summaryText += $"  Samples:     {currentPerformanceSummary.SampleCount}\n";
		if (currentPerformanceSummary.AvgMemoryMb.HasValue)
		{
			_summaryText += $"\n  Avg Memory:  {currentPerformanceSummary.AvgMemoryMb:F1} MB\n";
			_summaryText += $"  Peak Memory: {currentPerformanceSummary.PeakMemoryMb:F1} MB";
		}
		Debug.Log($"[Performance] Avg FPS: {currentPerformanceSummary.AvgFps:F1}, " + $"Min: {currentPerformanceSummary.MinFps:F1}, Max: {currentPerformanceSummary.MaxFps:F1}, " + $"1% Low: {currentPerformanceSummary.P1Fps:F1}, Samples: {currentPerformanceSummary.SampleCount}");
	}

	public void CheckPerformanceAndSuggest()
	{
		if (_isReady)
		{
			SessionPerformanceSummary currentPerformanceSummary = Analytics.GetCurrentPerformanceSummary();
			if (currentPerformanceSummary == null || currentPerformanceSummary.SampleCount < 60)
			{
				Debug.Log("[Performance] Not enough data yet, wait a few seconds");
			}
			else if (currentPerformanceSummary.AvgFps < 30f)
			{
				Debug.Log("[Performance] LOW FPS detected! Consider lowering quality:");
				Debug.Log("  - Reduce shadow quality");
				Debug.Log("  - Lower render resolution");
				Debug.Log("  - Disable post-processing effects");
			}
			else if (currentPerformanceSummary.AvgFps > 55f)
			{
				Debug.Log("[Performance] FPS is great! Game runs smoothly.");
			}
			else
			{
				Debug.Log($"[Performance] FPS is acceptable ({currentPerformanceSummary.AvgFps:F0} avg)");
			}
		}
	}

	private void OnGUI()
	{
		if (_isReady && showFpsOverlay)
		{
			Color textColor = ((_currentFps >= 55f) ? Color.green : ((_currentFps >= 30f) ? Color.yellow : Color.red));
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
			gUIStyle.fontSize = 20;
			gUIStyle.normal.textColor = textColor;
			GUI.Label(new Rect(10f, 200f, 200f, 30f), $"FPS: {_currentFps:F0}", gUIStyle);
			GUI.Label(new Rect(10f, 225f, 200f, 25f), $"Memory: {_currentMemoryMb:F0} MB");
			GUILayout.BeginArea(new Rect(Screen.width - 310, 10f, 300f, 350f));
			GUILayout.BeginVertical("box");
			GUILayout.Label("Performance Tracking Example");
			GUILayout.Label($"Current FPS: {_currentFps:F1}");
			GUILayout.Label($"Memory: {_currentMemoryMb:F1} MB");
			GUILayout.Space(10f);
			if (GUILayout.Button("Show Performance Summary"))
			{
				ShowPerformanceSummary();
			}
			if (GUILayout.Button("Check & Suggest Quality"))
			{
				CheckPerformanceAndSuggest();
			}
			GUILayout.Space(10f);
			if (!string.IsNullOrEmpty(_summaryText))
			{
				GUILayout.Label(_summaryText);
			}
			GUILayout.Space(5f);
			GUILayout.Label("Note: Performance data is sent");
			GUILayout.Label("automatically when session ends.");
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
}
