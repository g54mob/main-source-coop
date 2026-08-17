using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace EvilCore.Localization
{
	internal class MissingTranslationTracker
	{
		private readonly List<MissingTranslation> _missing = new List<MissingTranslation>();

		private readonly HashSet<string> _reportedKeys = new HashSet<string>();

		private readonly bool _logToConsole;

		public MissingTranslationTracker(bool logToConsole)
		{
			_logToConsole = logToConsole;
		}

		public void Track(string key, string localeCode)
		{
			string item = localeCode + ":" + key;
			if (_reportedKeys.Add(item))
			{
				string callerInfo = GetCallerInfo();
				_missing.Add(new MissingTranslation
				{
					Key = key,
					LocaleCode = localeCode,
					CallerInfo = callerInfo,
					Timestamp = DateTime.Now
				});
				if (_logToConsole)
				{
					Debug.LogWarning("[Localization] Missing key: \"" + key + "\" for locale \"" + localeCode + "\" (called from " + callerInfo + ")");
				}
			}
		}

		public IReadOnlyList<MissingTranslation> GetAll()
		{
			return _missing;
		}

		public void Clear()
		{
			_missing.Clear();
			_reportedKeys.Clear();
		}

		private static string GetCallerInfo()
		{
			StackFrame frame = new StackTrace(3, fNeedFileInfo: true).GetFrame(0);
			if (frame == null)
			{
				return "unknown";
			}
			MethodBase method = frame.GetMethod();
			if (method == null)
			{
				return "unknown";
			}
			return method.DeclaringType?.Name + "." + method.Name;
		}
	}
}
