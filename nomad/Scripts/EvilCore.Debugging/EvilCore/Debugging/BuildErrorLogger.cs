using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EvilCore.Debugging
{
	public class BuildErrorLogger : MonoBehaviour
	{
		private static BuildErrorLogger _instance;

		private static string _logFilePath;

		private static StringBuilder _logBuffer = new StringBuilder();

		private static float _lastFlushTime;

		private const float FLUSH_INTERVAL = 1f;

		private const int MAX_BUFFER_LENGTH = 262144;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			GameObject obj = new GameObject("[BuildErrorLogger]");
			UnityEngine.Object.DontDestroyOnLoad(obj);
			_instance = obj.AddComponent<BuildErrorLogger>();
			string text;
			try
			{
				text = Process.GetCurrentProcess().Id.ToString();
			}
			catch
			{
				text = Guid.NewGuid().ToString("N").Substring(0, 8);
			}
			_logFilePath = Path.Combine(Application.persistentDataPath, "build_error_log_" + text + ".txt");
			string text2 = new string('=', 60);
			string text3 = "\n" + text2 + "\n" + $"BUILD ERROR LOG - {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n" + "Unity Version: " + Application.unityVersion + "\n" + $"Platform: {Application.platform}\n" + text2 + "\n\n";
			try
			{
				AppendToFile(text3);
				Debug.Log("[BuildErrorLogger] Logging to: " + _logFilePath);
			}
			catch (Exception ex)
			{
				Debug.LogError("[BuildErrorLogger] Failed to create log file: " + ex.Message);
			}
			Application.logMessageReceived += OnLogMessageReceived;
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
			TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
		}

		private void OnDestroy()
		{
			Application.logMessageReceived -= OnLogMessageReceived;
			AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
			TaskScheduler.UnobservedTaskException -= OnUnobservedTaskException;
			FlushBuffer();
		}

		private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			AppendDiagnostic("AppDomain.UnhandledException", (e.ExceptionObject as Exception)?.ToString() ?? e.ExceptionObject?.ToString());
		}

		private static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			AppendDiagnostic("TaskScheduler.UnobservedTaskException", e.Exception?.ToString());
			e.SetObserved();
		}

		private static void AppendDiagnostic(string source, string detail)
		{
			string text = DateTime.Now.ToString("HH:mm:ss.fff");
			string value = "[" + text + "] [" + source + "]\n" + detail + "\n" + new string('-', 40) + "\n";
			lock (_logBuffer)
			{
				_logBuffer.Append(value);
			}
		}

		private void OnApplicationQuit()
		{
			FlushBuffer();
		}

		private void Update()
		{
			if (Time.realtimeSinceStartup - _lastFlushTime > 1f)
			{
				FlushBuffer();
				_lastFlushTime = Time.realtimeSinceStartup;
			}
		}

		private static void OnLogMessageReceived(string condition, string stackTrace, LogType type)
		{
			if (type != LogType.Error && type != LogType.Exception && type != LogType.Warning)
			{
				return;
			}
			string arg = DateTime.Now.ToString("HH:mm:ss.fff");
			string text = $"[{arg}] [{type}]\n{condition}\n";
			if (type == LogType.Error || type == LogType.Exception)
			{
				text = text + "StackTrace:\n" + stackTrace + "\n";
			}
			text = text + new string('-', 40) + "\n";
			lock (_logBuffer)
			{
				_logBuffer.Append(text);
			}
		}

		private static void FlushBuffer()
		{
			if (_logBuffer.Length == 0)
			{
				return;
			}
			string text;
			lock (_logBuffer)
			{
				text = _logBuffer.ToString();
				_logBuffer.Clear();
			}
			try
			{
				AppendToFile(text);
			}
			catch (Exception ex)
			{
				lock (_logBuffer)
				{
					if (_logBuffer.Length + text.Length <= 262144)
					{
						_logBuffer.Insert(0, text);
					}
				}
				Debug.LogError("[BuildErrorLogger] Failed to write log: " + ex.Message);
			}
		}

		private static void AppendToFile(string text)
		{
			using FileStream stream = new FileStream(_logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
			using StreamWriter streamWriter = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			streamWriter.Write(text);
		}
	}
}
