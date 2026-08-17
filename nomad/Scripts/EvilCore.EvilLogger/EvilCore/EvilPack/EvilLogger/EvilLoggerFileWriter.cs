using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace EvilCore.EvilPack.EvilLogger
{
	public static class EvilLoggerFileWriter
	{
		private static StreamWriter _writer;

		private static readonly Queue<string> _buffer = new Queue<string>();

		private static string _currentLogPath;

		private static long _currentFileSize;

		private static bool _isInitialized;

		private const int BUFFER_SIZE = 50;

		private const string LOG_FILE_PREFIX = "evil_log_";

		private const string LOG_FILE_EXTENSION = ".txt";

		public static bool IsInitialized => _isInitialized;

		public static void Initialize(string logDirectory)
		{
			if (_isInitialized)
			{
				return;
			}
			try
			{
				if (!Directory.Exists(logDirectory))
				{
					Directory.CreateDirectory(logDirectory);
				}
				string text = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
				_currentLogPath = Path.Combine(logDirectory, "evil_log_" + text + ".txt");
				_writer = new StreamWriter(_currentLogPath, append: true, Encoding.UTF8)
				{
					AutoFlush = false
				};
				_currentFileSize = 0L;
				_isInitialized = true;
				Application.quitting += OnApplicationQuit;
				Write($"=== EvilLogger Session Started: {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
			}
			catch (Exception ex)
			{
				Debug.LogError("[EvilLoggerFileWriter] Failed to initialize: " + ex.Message);
			}
		}

		public static void Write(string message)
		{
			if (_isInitialized)
			{
				_buffer.Enqueue($"[{DateTime.Now:HH:mm:ss.fff}] {message}");
				if (_buffer.Count >= 50)
				{
					Flush();
				}
			}
		}

		public static void WriteLog(LogLevel level, string scriptName, string memberName, string message, LogCategory category)
		{
			if (_isInitialized)
			{
				string text = level switch
				{
					LogLevel.Error => "ERR", 
					LogLevel.Warning => "WRN", 
					LogLevel.Info => "INF", 
					LogLevel.Verbose => "VRB", 
					_ => "???", 
				};
				string text2 = ((category != LogCategory.General && category != LogCategory.None) ? $"[{category}]" : "");
				string item = $"[{DateTime.Now:HH:mm:ss.fff}] [{text}]{text2} [{scriptName}.{memberName}] {message}";
				_buffer.Enqueue(item);
				if (_buffer.Count >= 50)
				{
					Flush();
				}
			}
		}

		public static void Flush()
		{
			if (!_isInitialized || _writer == null || _buffer.Count == 0)
			{
				return;
			}
			try
			{
				while (_buffer.Count > 0)
				{
					string text = _buffer.Dequeue();
					_writer.WriteLine(text);
					_currentFileSize += Encoding.UTF8.GetByteCount(text) + 2;
				}
				_writer.Flush();
			}
			catch (Exception ex)
			{
				Debug.LogError("[EvilLoggerFileWriter] Failed to flush: " + ex.Message);
			}
		}

		public static void CheckRotation(float maxFileSizeMB, int maxRotatedFiles, string logDirectory)
		{
			if (_isInitialized)
			{
				long num = (long)(maxFileSizeMB * 1024f * 1024f);
				if (_currentFileSize >= num)
				{
					RotateFile(maxRotatedFiles, logDirectory);
				}
			}
		}

		private static void RotateFile(int maxRotatedFiles, string logDirectory)
		{
			try
			{
				Flush();
				_writer?.Close();
				_writer = null;
				List<string> list = new List<string>(Directory.GetFiles(logDirectory, "evil_log_*.txt"));
				list.Sort();
				while (list.Count >= maxRotatedFiles)
				{
					File.Delete(list[0]);
					list.RemoveAt(0);
				}
				string text = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
				_currentLogPath = Path.Combine(logDirectory, "evil_log_" + text + ".txt");
				_writer = new StreamWriter(_currentLogPath, append: true, Encoding.UTF8)
				{
					AutoFlush = false
				};
				_currentFileSize = 0L;
				Write("=== Log Rotated ===");
			}
			catch (Exception ex)
			{
				Debug.LogError("[EvilLoggerFileWriter] Failed to rotate: " + ex.Message);
			}
		}

		public static void Shutdown()
		{
			if (!_isInitialized)
			{
				return;
			}
			try
			{
				Write($"=== EvilLogger Session Ended: {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
				Flush();
				_writer?.Close();
				_writer = null;
				_isInitialized = false;
			}
			catch (Exception ex)
			{
				Debug.LogError("[EvilLoggerFileWriter] Failed to shutdown: " + ex.Message);
			}
		}

		private static void OnApplicationQuit()
		{
			Shutdown();
			Application.quitting -= OnApplicationQuit;
		}

		public static string GetCurrentLogPath()
		{
			return _currentLogPath;
		}
	}
}
