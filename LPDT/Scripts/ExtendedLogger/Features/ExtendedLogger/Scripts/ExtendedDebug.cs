using UnityEngine;

namespace Features.ExtendedLogger.Scripts
{
	public static class ExtendedDebug
	{
		private static ExtendedDebugConfiguration _extendedDebugConfiguration;

		private static ExtendedDebugConfiguration ExtendedDebugConfiguration => _extendedDebugConfiguration ?? (_extendedDebugConfiguration = Resources.Load<ExtendedDebugConfiguration>("ExtendedDebugConfiguration"));

		public static void LogFiltered(DebugFilterType debugFilterType, object message, Object context = null)
		{
			if ((ExtendedDebugConfiguration.DebugFilterType & debugFilterType) != DebugFilterType.None)
			{
				if (!ExtendedDebugConfiguration.DebugFilterColors.TryGetValue(debugFilterType, out var value))
				{
					value = Color.white;
				}
				string arg = ColorUtility.ToHtmlStringRGB(value);
				string message2 = $"<color=#{arg}>[{debugFilterType}] {message}</color>";
				if (context != null)
				{
					Debug.LogError(message2, context);
				}
				else
				{
					Debug.LogError(message2);
				}
			}
		}
	}
}
