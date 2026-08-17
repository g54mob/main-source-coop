using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween
{
	internal static class Assert
	{
		internal static void LogErrorWithStackTrace(string msg, long id, [CanBeNull] object context)
		{
			Debug.LogError(TryAddStackTrace(msg, id), context as Object);
		}

		internal static void LogWarningWithStackTrace(string msg, long id, [CanBeNull] object context)
		{
			Debug.LogWarning(TryAddStackTrace(msg, id), context as Object);
		}

		[CanBeNull]
		[PublicAPI]
		private static string TryAddStackTrace([CanBeNull] string msg, long tweenId)
		{
			return msg;
		}
	}
}
