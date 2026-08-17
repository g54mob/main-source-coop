using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QFSW.QC
{
	public class LogStorage : ILogStorage
	{
		private readonly List<ILog> _consoleLogs = new List<ILog>(10);

		private readonly StringBuilder _logTraceBuilder = new StringBuilder(2048);

		public int MaxStoredLogs { get; set; }

		public IReadOnlyList<ILog> Logs => _consoleLogs;

		public LogStorage(int maxStoredLogs = -1)
		{
			MaxStoredLogs = maxStoredLogs;
		}

		public void AddLog(ILog log)
		{
			_consoleLogs.Add(log);
			int num = _logTraceBuilder.Length + log.Text.Length;
			if (log.NewLine && _logTraceBuilder.Length > 0)
			{
				num += Environment.NewLine.Length;
			}
			if (MaxStoredLogs > 0)
			{
				while (_consoleLogs.Count > MaxStoredLogs)
				{
					int num2 = _consoleLogs[0].Text.Length;
					if (_consoleLogs.Count > 1 && _consoleLogs[1].NewLine)
					{
						num2 += Environment.NewLine.Length;
					}
					num2 = Mathf.Min(num2, _logTraceBuilder.Length);
					num -= num2;
					_logTraceBuilder.Remove(0, num2);
					_consoleLogs.RemoveAt(0);
				}
			}
			int num3;
			for (num3 = _logTraceBuilder.Capacity; num3 < num; num3 *= 2)
			{
			}
			_logTraceBuilder.EnsureCapacity(num3);
			if (log.NewLine && _logTraceBuilder.Length > 0)
			{
				_logTraceBuilder.Append(Environment.NewLine);
			}
			_logTraceBuilder.Append(log.Text);
		}

		public void RemoveLog()
		{
			if (_consoleLogs.Count > 0)
			{
				ILog log = _consoleLogs[_consoleLogs.Count - 1];
				_consoleLogs.RemoveAt(_consoleLogs.Count - 1);
				int num = log.Text.Length;
				if (log.NewLine && _consoleLogs.Count > 0)
				{
					num += Environment.NewLine.Length;
				}
				_logTraceBuilder.Remove(_logTraceBuilder.Length - num, num);
			}
		}

		public void Clear()
		{
			_consoleLogs.Clear();
			_logTraceBuilder.Length = 0;
		}

		public string GetLogString()
		{
			return _logTraceBuilder.ToString();
		}
	}
}
