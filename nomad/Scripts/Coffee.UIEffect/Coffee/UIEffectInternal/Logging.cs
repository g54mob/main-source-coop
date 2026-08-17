using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace Coffee.UIEffectInternal
{
	internal static class Logging
	{
		private const string k_DisableSymbol = "DISABLE_COFFEE_LOGGER";

		[Conditional("DISABLE_COFFEE_LOGGER")]
		private static void Log_Internal(LogType type, object tag, object message, UnityEngine.Object context)
		{
		}

		[Conditional("DISABLE_COFFEE_LOGGER")]
		public static void LogIf(bool enable, object tag, object message, UnityEngine.Object context = null)
		{
		}

		[Conditional("DISABLE_COFFEE_LOGGER")]
		public static void Log(object tag, object message, UnityEngine.Object context = null)
		{
		}

		[Conditional("DISABLE_COFFEE_LOGGER")]
		public static void LogWarning(object tag, object message, UnityEngine.Object context = null)
		{
		}

		public static void LogError(object tag, object message, UnityEngine.Object context = null)
		{
			Debug.LogError($"{tag}: {message}", context);
		}

		[Conditional("DISABLE_COFFEE_LOGGER")]
		public static void LogMulticast(Type type, string fieldName, object instance = null, string message = null)
		{
		}

		[Conditional("DISABLE_COFFEE_LOGGER")]
		private static void AppendTag(StringBuilder sb, object tag)
		{
		}

		[Conditional("DISABLE_COFFEE_LOGGER")]
		private static void AppendType(StringBuilder sb, Type type)
		{
		}

		[Conditional("DISABLE_COFFEE_LOGGER")]
		private static void AppendReadableCode(StringBuilder sb, object tag)
		{
		}
	}
}
