using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace VisualDesignCafe.Nature
{
	internal class Logger : ILog
	{
		private readonly string _prefix;

		private int _indent;

		private Stack<Stopwatch> _timers = new Stack<Stopwatch>();

		public long ElapsedMilliseconds => _timers.Peek().ElapsedMilliseconds;

		public bool Debug { get; set; }

		public Logger(string prefix)
		{
			_prefix = prefix;
		}

		public void StartTimer()
		{
			Stopwatch stopwatch = new Stopwatch();
			_timers.Push(stopwatch);
			stopwatch.Start();
		}

		public long StopTimer()
		{
			Stopwatch stopwatch = _timers.Pop();
			stopwatch.Stop();
			return stopwatch.ElapsedMilliseconds;
		}

		public void Indent()
		{
			_indent++;
		}

		public void Unindent()
		{
			_indent--;
		}

		public void Log(string message)
		{
			UnityEngine.Debug.Log(_prefix + GetIndent() + message);
		}

		public void LogFormat(string message, params object[] args)
		{
			UnityEngine.Debug.LogFormat(_prefix + GetIndent() + message, args);
		}

		public void LogWarning(string message)
		{
			UnityEngine.Debug.LogWarning(_prefix + GetIndent() + message);
		}

		public void LogWarningFormat(string message, params object[] args)
		{
			UnityEngine.Debug.LogWarningFormat(_prefix + GetIndent() + message, args);
		}

		public void LogError(string message)
		{
			UnityEngine.Debug.LogError(_prefix + GetIndent() + message);
		}

		public void LogErrorFormat(string message, params object[] args)
		{
			UnityEngine.Debug.LogErrorFormat(_prefix + GetIndent() + message, args);
		}

		private string GetIndent()
		{
			if (!Debug)
			{
				return string.Empty;
			}
			if (_indent == 0)
			{
				return string.Empty;
			}
			return new string(' ', _indent * 4);
		}
	}
}
