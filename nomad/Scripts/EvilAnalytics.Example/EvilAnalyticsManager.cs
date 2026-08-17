using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EvilAnalytics.SDK.Core;
using EvilAnalytics.Shared.BugReports;
using EvilAnalytics.Shared.Common;
using EvilAnalytics.Shared.Crashlytics;
using EvilAnalytics.Shared.Events;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

public class EvilAnalyticsManager : MonoBehaviour
{
	[Header("Server Configuration")]
	[SerializeField]
	private string serverUrl = "http://localhost:5000";

	[SerializeField]
	private string apiKey = "YOUR_API_KEY_HERE";

	[Header("SDK Settings")]
	[SerializeField]
	private bool enableCompression = true;

	[SerializeField]
	private bool debugLogging = true;

	[Tooltip("Disable for local HTTP development. Enable for production HTTPS.")]
	[SerializeField]
	private bool requireHttps;

	[Header("GDPR Consent")]
	[Tooltip("Auto-grant all consents on start. In production, show a consent dialog first.")]
	[SerializeField]
	private bool autoGrantConsent = true;

	[Header("Heartbeat Settings")]
	[SerializeField]
	private bool enableHeartbeat = true;

	[SerializeField]
	private float heartbeatInterval = 30f;

	[Tooltip("Keep sending heartbeats when game is not focused (prevents session timeout)")]
	[SerializeField]
	private bool runInBackground = true;

	[Header("Performance Tracking")]
	[SerializeField]
	private bool enablePerformanceTracking = true;

	[Header("Bug Report Settings")]
	[Tooltip("Enable F8 key to open bug report dialog")]
	[SerializeField]
	private bool enableBugReportKey = true;

	[Tooltip("Key to trigger bug report (default: F8)")]
	[SerializeField]
	private KeyCode bugReportKey = KeyCode.F8;

	[Tooltip("Include screenshot with bug report")]
	[SerializeField]
	private bool bugReportIncludeScreenshot = true;

	[Header("Crashlytics Settings")]
	[Tooltip("Enable automatic error/exception/warning capture")]
	[SerializeField]
	private bool enableCrashlytics = true;

	[Tooltip("Minimum severity level to capture. Error = only errors and exceptions, Warning = include warnings")]
	[SerializeField]
	private CrashlyticsMinSeverity crashlyticsMinSeverity;

	[Tooltip("How often to send crash logs to server (seconds)")]
	[SerializeField]
	private float crashlyticsFlushInterval = 30f;

	[Tooltip("Deduplicate identical errors within time window (prevents flooding from Update loop errors)")]
	[SerializeField]
	private bool crashlyticsDeduplication = true;

	[Tooltip("Time window for error deduplication (seconds)")]
	[Range(1f, 60f)]
	[SerializeField]
	private float crashlyticsDeduplicationWindow = 5f;

	private bool _isInitialized;

	private bool _showBugReportDialog;

	private string _bugReportMessage = "";

	private bool _isSendingBugReport;

	private bool _previousCursorVisible = true;

	private CursorLockMode _previousCursorLockState;

	private float _heartbeatTimer;

	private float _performanceTimer;

	private float _crashlyticsTimer;

	private float _lastFps;

	private float _lastMemoryMb;

	[Header("Debug")]
	[SerializeField]
	private bool showDebugUI = true;

	public static EvilAnalyticsManager Instance { get; private set; }

	public bool IsReady => _isInitialized;

	public event Action<bool> OnInitialized;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (runInBackground)
		{
			Application.runInBackground = true;
		}
	}

	private async void Start()
	{
		await InitializeAnalytics();
	}

	private void Update()
	{
		if (!_isInitialized)
		{
			return;
		}
		if (enablePerformanceTracking)
		{
			_lastFps = 1f / Time.unscaledDeltaTime;
			Analytics.RecordFps(_lastFps);
			_performanceTimer += Time.deltaTime;
			if (_performanceTimer >= 1f)
			{
				_performanceTimer = 0f;
				_lastMemoryMb = (float)Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
				Analytics.UpdateMemoryUsage(_lastMemoryMb);
			}
		}
		if (enableCrashlytics)
		{
			_crashlyticsTimer += Time.deltaTime;
			if (_crashlyticsTimer >= crashlyticsFlushInterval)
			{
				_crashlyticsTimer = 0f;
				FlushCrashLogs();
			}
		}
		if (enableHeartbeat)
		{
			_heartbeatTimer += Time.deltaTime;
			if (_heartbeatTimer >= 1f)
			{
				_heartbeatTimer = 0f;
				TrySendHeartbeat();
			}
		}
		if (enableBugReportKey && GetBugReportKeyDown())
		{
			_showBugReportDialog = !_showBugReportDialog;
			if (_showBugReportDialog)
			{
				_bugReportMessage = "";
				ShowCursor();
			}
			else
			{
				RestoreCursor();
			}
		}
	}

	private bool GetBugReportKeyDown()
	{
		if (Keyboard.current != null)
		{
			return Keyboard.current[KeyToInputSystemKey(bugReportKey)].wasPressedThisFrame;
		}
		return false;
	}

	private void ShowCursor()
	{
		_previousCursorVisible = Cursor.visible;
		_previousCursorLockState = Cursor.lockState;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	private void RestoreCursor()
	{
		Cursor.visible = _previousCursorVisible;
		Cursor.lockState = _previousCursorLockState;
	}

	private Key KeyToInputSystemKey(KeyCode keyCode)
	{
		return keyCode switch
		{
			KeyCode.F1 => Key.F1, 
			KeyCode.F2 => Key.F2, 
			KeyCode.F3 => Key.F3, 
			KeyCode.F4 => Key.F4, 
			KeyCode.F5 => Key.F5, 
			KeyCode.F6 => Key.F6, 
			KeyCode.F7 => Key.F7, 
			KeyCode.F8 => Key.F8, 
			KeyCode.F9 => Key.F9, 
			KeyCode.F10 => Key.F10, 
			KeyCode.F11 => Key.F11, 
			KeyCode.F12 => Key.F12, 
			KeyCode.Escape => Key.Escape, 
			KeyCode.Space => Key.Space, 
			KeyCode.Return => Key.Enter, 
			KeyCode.Tab => Key.Tab, 
			KeyCode.Backspace => Key.Backspace, 
			_ => Key.F8, 
		};
	}

	private async Task TrySendHeartbeat()
	{
		try
		{
			await Analytics.TrySendHeartbeatAsync();
		}
		catch (Exception ex)
		{
			if (debugLogging)
			{
				Debug.LogWarning("[EvilAnalytics] Heartbeat error: " + ex.Message);
			}
		}
	}

	private async Task FlushCrashLogs()
	{
		try
		{
			await Analytics.FlushCrashLogsAsync(Application.platform.ToString(), Application.version);
		}
		catch (Exception ex)
		{
			if (debugLogging)
			{
				Debug.LogWarning("[EvilAnalytics] Crashlytics flush error: " + ex.Message);
			}
		}
	}

	private async Task FlushCrashLogsImmediate()
	{
		try
		{
			if (await Analytics.FlushCrashLogsAsync(Application.platform.ToString(), Application.version) && debugLogging)
			{
				Debug.Log("[EvilAnalytics] Critical error sent to server immediately");
			}
		}
		catch (Exception ex)
		{
			if (debugLogging)
			{
				Debug.LogWarning("[EvilAnalytics] Failed to send critical error: " + ex.Message);
			}
		}
	}

	private async Task InitializeAnalytics()
	{
		if (_isInitialized)
		{
			return;
		}
		try
		{
			if (debugLogging)
			{
				Analytics.SetLogger(delegate(string msg)
				{
					Debug.Log(msg);
				});
			}
			EvilAnalyticsConfig config = new EvilAnalyticsConfig
			{
				ApiKey = apiKey,
				ServerUrl = serverUrl,
				EnableCompression = enableCompression,
				EnableDebugLogging = debugLogging,
				EnableHeartbeat = enableHeartbeat,
				HeartbeatInterval = heartbeatInterval,
				EnablePerformanceTracking = enablePerformanceTracking,
				EnableCrashlytics = enableCrashlytics,
				RequireHttps = requireHttps
			};
			string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
			await Analytics.InitializeAsync(config, deviceUniqueIdentifier);
			if (enableCrashlytics)
			{
				Analytics.SetCrashlyticsMinSeverity((crashlyticsMinSeverity != CrashlyticsMinSeverity.Error) ? LogSeverity.Warning : LogSeverity.Error);
				Analytics.SetCrashlyticsDeduplication(crashlyticsDeduplication, crashlyticsDeduplicationWindow);
				Analytics.SetCrashlyticsContextCallbacks(() => SceneManager.GetActiveScene().name, () => (!(_lastFps > 0f)) ? ((float?)null) : new float?(_lastFps), () => (!(_lastMemoryMb > 0f)) ? ((float?)null) : new float?(_lastMemoryMb));
				Analytics.SetOnCriticalErrorCallback(delegate(LogSeverity severity)
				{
					if (debugLogging)
					{
						Debug.Log($"[EvilAnalytics] Critical error detected ({severity}), flushing immediately...");
					}
					FlushCrashLogsImmediate();
				});
				Application.logMessageReceived += OnLogMessageReceived;
			}
			if (autoGrantConsent)
			{
				Analytics.GrantConsent();
			}
			await Analytics.StartSessionAsync(Application.platform.ToString());
			await SendUnityHardwareInfo();
			_isInitialized = true;
			Debug.Log("[EvilAnalytics] SDK initialized successfully!");
			this.OnInitialized?.Invoke(obj: true);
		}
		catch (Exception ex)
		{
			Debug.LogError("[EvilAnalytics] Failed to initialize: " + ex.Message);
			this.OnInitialized?.Invoke(obj: false);
		}
	}

	public IEnumerator WaitForInitialization()
	{
		while (!_isInitialized)
		{
			yield return null;
		}
	}

	private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
	{
		if (_isInitialized && enableCrashlytics)
		{
			LogSeverity severity;
			switch (type)
			{
			default:
				return;
			case LogType.Error:
				severity = LogSeverity.Error;
				break;
			case LogType.Exception:
				severity = LogSeverity.Exception;
				break;
			case LogType.Assert:
				severity = LogSeverity.Assert;
				break;
			case LogType.Warning:
				severity = LogSeverity.Warning;
				break;
			case LogType.Log:
				return;
			}
			if (!condition.StartsWith("[EvilAnalytics]"))
			{
				Analytics.RecordLog(condition, stackTrace, severity);
			}
		}
	}

	private void OnDestroy()
	{
		if (enableCrashlytics)
		{
			Application.logMessageReceived -= OnLogMessageReceived;
		}
	}

	private async Task SendUnityHardwareInfo()
	{
		try
		{
			await Analytics.SendHardwareInfoAsync(new HardwareInfo
			{
				CollectedAt = DateTimeOffset.UtcNow,
				DeviceModel = SystemInfo.deviceModel,
				DeviceType = SystemInfo.deviceType.ToString(),
				DeviceName = SystemInfo.deviceName,
				OperatingSystem = SystemInfo.operatingSystem,
				OperatingSystemFamily = SystemInfo.operatingSystemFamily.ToString(),
				ProcessorType = SystemInfo.processorType,
				ProcessorCount = SystemInfo.processorCount,
				ProcessorFrequencyMHz = SystemInfo.processorFrequency,
				SystemMemoryMB = SystemInfo.systemMemorySize,
				GraphicsDeviceName = SystemInfo.graphicsDeviceName,
				GraphicsDeviceVendor = SystemInfo.graphicsDeviceVendor,
				GraphicsDeviceType = SystemInfo.graphicsDeviceType.ToString(),
				GraphicsMemoryMB = SystemInfo.graphicsMemorySize,
				GraphicsShaderLevel = SystemInfo.graphicsShaderLevel,
				ScreenWidth = Screen.width,
				ScreenHeight = Screen.height,
				ScreenDpi = Screen.dpi,
				ScreenRefreshRate = (int)((Screen.currentResolution.refreshRateRatio.numerator != 0) ? (Screen.currentResolution.refreshRateRatio.numerator / Screen.currentResolution.refreshRateRatio.denominator) : 60),
				FullScreen = Screen.fullScreen,
				SupportsGyroscope = SystemInfo.supportsGyroscope,
				SupportsAccelerometer = SystemInfo.supportsAccelerometer,
				SupportsLocationService = SystemInfo.supportsLocationService,
				SupportsVibration = SystemInfo.supportsVibration,
				SupportsAudio = SystemInfo.supportsAudio,
				BatteryLevel = SystemInfo.batteryLevel,
				BatteryStatus = SystemInfo.batteryStatus.ToString()
			});
			Debug.Log("[EvilAnalytics] Hardware info sent successfully");
		}
		catch (Exception ex)
		{
			Debug.LogWarning("[EvilAnalytics] Failed to send hardware info: " + ex.Message);
		}
	}

	public async void TrackEvent(string eventName)
	{
		if (_isInitialized)
		{
			await Analytics.TrackEventAsync(eventName);
		}
	}

	public async void TrackEvent(string eventName, Dictionary<string, object> properties)
	{
		if (_isInitialized)
		{
			await Analytics.TrackEventAsync(eventName, properties);
		}
	}

	public async void TrackEvent(string eventName, EventCategory category, Dictionary<string, object> properties = null)
	{
		if (_isInitialized)
		{
			await Analytics.TrackEventAsync(eventName, category, properties);
		}
	}

	public async void TrackLevelStart(int levelId, string levelName = null)
	{
		if (_isInitialized)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object> { { "level_id", levelId } };
			if (!string.IsNullOrEmpty(levelName))
			{
				dictionary["level_name"] = levelName;
			}
			await Analytics.TrackLevelEventAsync("level_start", levelId, dictionary);
		}
	}

	public async void TrackLevelComplete(int levelId, float completionTime, int score = 0, int stars = 0)
	{
		if (_isInitialized)
		{
			Dictionary<string, object> properties = new Dictionary<string, object>
			{
				{ "level_id", levelId },
				{ "completion_time", completionTime },
				{ "score", score },
				{ "stars", stars }
			};
			await Analytics.TrackLevelEventAsync("level_complete", levelId, properties);
		}
	}

	public async void TrackLevelFailed(int levelId, float playTime, string failReason = null)
	{
		if (_isInitialized)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{ "level_id", levelId },
				{ "play_time", playTime }
			};
			if (!string.IsNullOrEmpty(failReason))
			{
				dictionary["fail_reason"] = failReason;
			}
			await Analytics.TrackLevelEventAsync("level_failed", levelId, dictionary);
		}
	}

	public async void TrackPurchase(string itemId, string itemName, decimal price, string currency = "USD")
	{
		if (_isInitialized)
		{
			Dictionary<string, object> properties = new Dictionary<string, object>
			{
				{ "item_id", itemId },
				{ "item_name", itemName },
				{ "currency", currency }
			};
			await Analytics.TrackEventWithValueAsync("purchase", price, EventCategory.Monetization, properties);
		}
	}

	public async void TrackAdShown(string adType, string adNetwork, string placement)
	{
		if (_isInitialized)
		{
			await Analytics.TrackEventAsync("ad_shown", EventCategory.Monetization, new Dictionary<string, object>
			{
				{ "ad_type", adType },
				{ "ad_network", adNetwork },
				{ "placement", placement }
			});
		}
	}

	public async void TrackTutorialStep(int step, string stepName)
	{
		if (_isInitialized)
		{
			await Analytics.TrackEventAsync("tutorial_step", EventCategory.Progression, new Dictionary<string, object>
			{
				{ "step", step },
				{ "step_name", stepName }
			});
		}
	}

	public async void TrackTutorialComplete()
	{
		if (_isInitialized)
		{
			await Analytics.TrackEventAsync("tutorial_complete", EventCategory.Progression);
		}
	}

	public async void TrackEventWithValue(string eventName, decimal value, EventCategory category = EventCategory.Custom, Dictionary<string, object> properties = null)
	{
		if (_isInitialized)
		{
			await Analytics.TrackEventWithValueAsync(eventName, value, category, properties);
		}
	}

	public void SubmitBugReport(string message, bool includeScreenshot = false, Action<bool, Guid?> callback = null)
	{
		if (!_isInitialized)
		{
			callback?.Invoke(arg1: false, null);
		}
		else
		{
			StartCoroutine(SubmitBugReportCoroutine(message, includeScreenshot, callback));
		}
	}

	private IEnumerator SubmitBugReportCoroutine(string message, bool includeScreenshot, Action<bool, Guid?> callback)
	{
		byte[] array = null;
		if (includeScreenshot)
		{
			bool wasDialogVisible = _showBugReportDialog;
			_showBugReportDialog = false;
			yield return new WaitForEndOfFrame();
			try
			{
				Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, mipChain: false);
				texture2D.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
				texture2D.Apply();
				array = texture2D.EncodeToPNG();
				UnityEngine.Object.Destroy(texture2D);
				if (debugLogging)
				{
					Debug.Log($"[EvilAnalytics] Screenshot captured: {array.Length / 1024}KB");
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[EvilAnalytics] Failed to capture screenshot: " + ex.Message);
				array = null;
			}
			_showBugReportDialog = wasDialogVisible;
		}
		string sceneName = SceneManager.GetActiveScene().name;
		SystemResourceSnapshot systemResources = CollectSystemResources();
		List<LogEntry> recentCrashLogs = Analytics.GetRecentCrashLogs();
		var value = new
		{
			message = message,
			sceneName = sceneName,
			gameVersion = Application.version,
			platform = Application.platform.ToString(),
			systemResources = systemResources,
			recentLogs = recentCrashLogs,
			unityVersion = Application.unityVersion,
			isEditor = Application.isEditor,
			timestamp = DateTimeOffset.UtcNow
		};
		string fullReportJson = null;
		try
		{
			fullReportJson = JsonConvert.SerializeObject(value);
		}
		catch
		{
		}
		Task<BugReportResponse> task = SubmitBugReportWithExtendedDataAsync(message, array, sceneName, Application.version, systemResources, recentCrashLogs, fullReportJson);
		while (!task.IsCompleted)
		{
			yield return null;
		}
		if (task.Exception != null)
		{
			Debug.LogError("[EvilAnalytics] Bug report failed: " + task.Exception.Message);
			callback?.Invoke(arg1: false, null);
			yield break;
		}
		BugReportResponse result = task.Result;
		bool flag = result?.Success ?? false;
		if (flag)
		{
			Debug.Log($"[EvilAnalytics] Bug report submitted: {result.BugReportId}");
		}
		else
		{
			Debug.LogWarning("[EvilAnalytics] Bug report submission failed");
		}
		callback?.Invoke(flag, (!flag) ? ((Guid?)null) : result?.BugReportId);
	}

	private SystemResourceSnapshot CollectSystemResources()
	{
		return Analytics.CollectSystemResources(SystemInfo.systemMemorySize, (long)((_lastMemoryMb > 0f) ? _lastMemoryMb : ((float)Profiler.GetTotalAllocatedMemoryLong() / 1048576f)), Profiler.GetTotalAllocatedMemoryLong() / 1048576, Profiler.GetTotalReservedMemoryLong() / 1048576, GC.GetTotalMemory(forceFullCollection: false) / 1048576, Screen.width, Screen.height, (int)((Screen.currentResolution.refreshRateRatio.numerator != 0) ? (Screen.currentResolution.refreshRateRatio.numerator / Screen.currentResolution.refreshRateRatio.denominator) : 60), Screen.fullScreen, QualitySettings.names[QualitySettings.GetQualityLevel()], SystemInfo.graphicsDeviceName, SystemInfo.graphicsMemorySize, SystemInfo.processorType, SystemInfo.processorCount, SystemInfo.processorFrequency);
	}

	private async Task<BugReportResponse> SubmitBugReportWithExtendedDataAsync(string message, byte[] screenshot, string sceneName, string gameVersion, SystemResourceSnapshot systemResources, List<LogEntry> recentLogs, string fullReportJson)
	{
		if (!_isInitialized)
		{
			return null;
		}
		return await Analytics.SubmitBugReportWithExtendedDataAsync(message, screenshot, sceneName, gameVersion, systemResources, recentLogs, fullReportJson);
	}

	public async Task<BugReportResponse> SubmitBugReportAsync(string message, byte[] screenshot = null)
	{
		if (!_isInitialized)
		{
			return null;
		}
		string sceneName = SceneManager.GetActiveScene().name;
		return await Analytics.SubmitBugReportAsync(message, screenshot, sceneName, Application.version);
	}

	private void OnGUI()
	{
		if (_showBugReportDialog && _isInitialized)
		{
			DrawBugReportDialog();
		}
		if (!showDebugUI)
		{
			return;
		}
		GUILayout.BeginArea(new Rect(10f, 10f, 300f, 170f));
		GUILayout.BeginVertical("box");
		GUILayout.Label("EvilAnalytics Debug");
		GUILayout.Label($"Initialized: {_isInitialized}");
		GUILayout.Label($"Session Active: {Analytics.CurrentSessionId.HasValue}");
		GUILayout.Label($"Bug Report Key: {bugReportKey}");
		if (_isInitialized && GUILayout.Button("Send Test Event"))
		{
			TrackEvent("debug_test_event", new Dictionary<string, object> { 
			{
				"timestamp",
				DateTime.Now.ToString()
			} });
			Debug.Log("[EvilAnalytics] Test event sent!");
		}
		if (_isInitialized && GUILayout.Button("Submit Test Bug Report"))
		{
			SubmitBugReport("Test bug report from debug UI", includeScreenshot: true, delegate(bool success, Guid? id)
			{
				Debug.Log($"[EvilAnalytics] Bug report result: success={success}, id={id}");
			});
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void DrawBugReportDialog()
	{
		float num = 600f;
		float num2 = 400f;
		float num3 = ((float)Screen.width - num) / 2f;
		float num4 = ((float)Screen.height - num2) / 2f;
		GUI.color = new Color(0f, 0f, 0f, 0.5f);
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
		GUI.color = Color.white;
		GUI.Box(new Rect(num3 - 10f, num4 - 10f, num + 20f, num2 + 20f), "");
		GUILayout.BeginArea(new Rect(num3, num4, num, num2));
		GUILayout.BeginVertical("box");
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
		gUIStyle.fontSize = 18;
		gUIStyle.fontStyle = FontStyle.Bold;
		GUILayout.Label("Bug Report", gUIStyle);
		GUILayout.Label($"Press {bugReportKey} to close", GUI.skin.label);
		GUILayout.Space(15f);
		GUILayout.Label("Describe the issue in detail:");
		_bugReportMessage = GUILayout.TextArea(_bugReportMessage, GUILayout.Height(180f));
		GUILayout.Space(15f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Screenshot: " + (bugReportIncludeScreenshot ? "Will be included" : "Not included"));
		GUILayout.FlexibleSpace();
		GUILayout.Label("Scene: " + SceneManager.GetActiveScene().name);
		GUILayout.EndHorizontal();
		GUILayout.Space(15f);
		GUILayout.BeginHorizontal();
		GUI.enabled = !_isSendingBugReport && !string.IsNullOrWhiteSpace(_bugReportMessage);
		if (GUILayout.Button(_isSendingBugReport ? "Sending..." : "Submit Bug Report", GUILayout.Height(40f), GUILayout.MinWidth(200f)))
		{
			_isSendingBugReport = true;
			SubmitBugReport(_bugReportMessage, bugReportIncludeScreenshot, delegate(bool success, Guid? id)
			{
				_isSendingBugReport = false;
				if (success)
				{
					Debug.Log($"[EvilAnalytics] Bug report submitted: {id}");
					_showBugReportDialog = false;
					_bugReportMessage = "";
					RestoreCursor();
				}
				else
				{
					Debug.LogWarning("[EvilAnalytics] Bug report failed");
				}
			});
		}
		GUI.enabled = true;
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Cancel", GUILayout.Height(40f), GUILayout.MinWidth(120f)))
		{
			_showBugReportDialog = false;
			_bugReportMessage = "";
			RestoreCursor();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (_isInitialized && !pauseStatus && enableHeartbeat)
		{
			if (debugLogging)
			{
				Debug.Log("[EvilAnalytics] App resumed, sending heartbeat...");
			}
			Analytics.SendHeartbeatAsync();
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (_isInitialized && hasFocus && enableHeartbeat)
		{
			if (debugLogging)
			{
				Debug.Log("[EvilAnalytics] App focused, sending heartbeat...");
			}
			Analytics.SendHeartbeatAsync();
		}
	}

	private async void OnApplicationQuit()
	{
		if (_isInitialized)
		{
			Debug.Log("[EvilAnalytics] Ending session...");
			await Analytics.EndSessionAsync();
			Debug.Log("[EvilAnalytics] Session ended");
		}
	}
}
