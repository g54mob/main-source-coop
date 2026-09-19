using System;
using System.Diagnostics;
using UnityEngine;

namespace Mirror.SimpleWeb
{
	public static class Log
	{
		public enum Levels
		{
			Flood = 0,
			Verbose = 1,
			Info = 2,
			Warn = 3,
			Error = 4,
			None = 5
		}

		public static ILogger logger = UnityEngine.Debug.unityLogger;

		public static Levels minLogLevel = Levels.None;

		public static void Exception(Exception e)
		{
			logger.Log(LogType.Exception, "[SWT:Exception] " + e.GetType().Name + ": " + e.Message + "\n" + e.StackTrace + "\n\n");
		}

		[Conditional("DEBUG")]
		public static void Flood(string msg)
		{
			if (minLogLevel <= Levels.Flood)
			{
				logger.Log(LogType.Log, msg.Trim());
			}
		}

		[Conditional("DEBUG")]
		public static void DumpBuffer(string label, byte[] buffer, int offset, int length)
		{
			if (minLogLevel <= Levels.Flood)
			{
				logger.Log(LogType.Log, "<color=cyan>" + label + ": " + BufferToString(buffer, offset, length) + "</color>");
			}
		}

		[Conditional("DEBUG")]
		public static void DumpBuffer(string label, ArrayBuffer arrayBuffer)
		{
			if (minLogLevel <= Levels.Flood)
			{
				logger.Log(LogType.Log, "<color=cyan>" + label + ": " + BufferToString(arrayBuffer.array, 0, arrayBuffer.count) + "</color>");
			}
		}

		public static void Verbose(string msg)
		{
			if (minLogLevel <= Levels.Verbose)
			{
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine(msg.Trim());
				Console.ResetColor();
			}
		}

		public static void Verbose<T>(string msg, T arg1)
		{
			if (minLogLevel <= Levels.Verbose)
			{
				Verbose(string.Format(msg, arg1));
			}
		}

		public static void Verbose<T1, T2>(string msg, T1 arg1, T2 arg2)
		{
			if (minLogLevel <= Levels.Verbose)
			{
				Verbose(string.Format(msg, arg1, arg2));
			}
		}

		private static void Info(string msg, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			Console.ForegroundColor = consoleColor;
			Console.WriteLine(msg.Trim());
			Console.ResetColor();
		}

		public static void Info<T>(string msg, T arg1, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			if (minLogLevel <= Levels.Info)
			{
				Info(string.Format(msg, arg1), consoleColor);
			}
		}

		public static void Info<T1, T2>(string msg, T1 arg1, T2 arg2, ConsoleColor consoleColor = ConsoleColor.Cyan)
		{
			if (minLogLevel <= Levels.Info)
			{
				Info(string.Format(msg, arg1, arg2), consoleColor);
			}
		}

		public static void InfoException(Exception e)
		{
			if (minLogLevel <= Levels.Info)
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine(e.Message);
				Console.ResetColor();
			}
		}

		public static void Warn(string msg)
		{
			if (minLogLevel <= Levels.Warn)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(msg.Trim());
				Console.ResetColor();
			}
		}

		public static void Warn<T>(string msg, T arg1)
		{
			if (minLogLevel <= Levels.Warn)
			{
				Warn(string.Format(msg, arg1));
			}
		}

		public static void Error(string msg)
		{
			if (minLogLevel <= Levels.Error)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(msg.Trim());
				Console.ResetColor();
			}
		}

		public static void Error<T>(string msg, T arg1)
		{
			if (minLogLevel <= Levels.Error)
			{
				Error(string.Format(msg, arg1));
			}
		}

		public static void Error<T1, T2>(string msg, T1 arg1, T2 arg2)
		{
			if (minLogLevel <= Levels.Error)
			{
				Error(string.Format(msg, arg1, arg2));
			}
		}

		public static void Error<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3)
		{
			if (minLogLevel <= Levels.Error)
			{
				Error(string.Format(msg, arg1, arg2, arg3));
			}
		}

		public static string BufferToString(byte[] buffer, int offset = 0, int? length = null)
		{
			return BitConverter.ToString(buffer, offset, length ?? buffer.Length);
		}
	}
}
