using UnityEngine;

public static class GameBooter
{
	private static bool m_initialized;

	public static bool Initialized => m_initialized;

	public static void Initialize()
	{
		if (!m_initialized)
		{
			m_initialized = true;
			new GameObject("Game").AddComponent<GameHandler>().Initialize();
		}
	}
}
